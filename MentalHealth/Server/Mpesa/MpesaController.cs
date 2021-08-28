using System;
using System.IO;
using System.Threading.Tasks;
using MentalHealth.Server.Data;
using MentalHealth.Shared.Models.UserAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MentalHealth.Server.Mpesa
{
    [Route("api/[controller]")]
    [ApiController]
    public class MpesaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        readonly MpesaRequests requests;

        public MpesaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            requests = new();
        }

        public async Task<IActionResult> Api()
        {
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                MpesaTransaction transaction = new MpesaTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = "MpesaResponse",
                    TransactionType = "Mpesa Response",
                    Data = await stream.ReadToEndAsync()
                };
                _context.Add(transaction);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize(Roles = "HealthOfficer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(string phoneNumber, [FromForm] string amount)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound("User not found");
                var mpesaAccount = await _context.MpesaAccounts.FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (mpesaAccount == null) return NotFound();

                if (mpesaAccount.AccountBalance < Convert.ToDecimal(amount))
                    return BadRequest("You have insufficient amount in your account");

                //Make payment request using Lipa Na Mpesa
                string url = "";
                var response = await requests.B2C(requests.AccessToken(), amount, phoneNumber, url);

                if (string.IsNullOrEmpty(response))
                    return BadRequest("An error has occured. Please try again. If the error persists, try again later");

                var error = JObject.Parse(response)["errorMessage"].ToString();
                if (error != null) return BadRequest(error);

                MpesaTransaction transaction = new MpesaTransaction
                {
                    Id=Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    TransactionType = "Business To Customer",
                    Data = response
                };

                _context.Add(transaction);
                mpesaAccount.AccountBalance -= Convert.ToDecimal(amount);
                await _context.SaveChangesAsync();
                return Ok("Widrawal successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
