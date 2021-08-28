using MentalHealth.Server.Data;
using MentalHealth.Server.Mpesa;
using MentalHealth.Shared.Models;
using MentalHealth.Shared.Models.UserAccount;
using MentalHealth.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MentalHealth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HealthRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MpesaRequests requests;

        public HealthRecordsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            requests = new();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (User.IsInRole("User"))
            {
                var sessions = await _context.SessionRecords.Where(x => x.PatientId == user.Id).ToListAsync();
                foreach (var session in sessions)
                {
                    var healthOfficer = await _userManager.FindByIdAsync(session.HealthOfficerId);
                    session.HealthOfficer = new UserDetails
                    {
                        Id = healthOfficer.Id,
                        UserName = healthOfficer.UserName,
                        Email = healthOfficer.Email,
                        EmailConfirmed = healthOfficer.EmailConfirmed,
                        Phonenumber = healthOfficer.PhoneNumber,
                        PhonenumberConfirmed = healthOfficer.PhoneNumberConfirmed,
                        FirstName = healthOfficer.FirstName,
                        Surname = healthOfficer.Surname,
                        OtherNames = healthOfficer.OtherNames,
                        IdNo = healthOfficer.IdNo,
                        DateCreated = healthOfficer.DateCreated,
                        LocationLatitude = healthOfficer.LocationLatitude,
                        LocationLongitude = healthOfficer.LocationLongitude,
                        IsOccupied = healthOfficer.IsOccupied
                    };
                }
                return Ok(sessions);
            }
            if (User.IsInRole("HealthOfficer"))
            {
                var sessions = await _context.SessionRecords.Where(x => x.HealthOfficerId == user.Id).ToListAsync();
                foreach (var session in sessions)
                {
                    var patient = await _userManager.FindByIdAsync(session.PatientId);
                    session.Patient = new UserDetails
                    {
                        Id = patient.Id,
                        UserName = patient.UserName,
                        Email = patient.Email,
                        EmailConfirmed = patient.EmailConfirmed,
                        Phonenumber = patient.PhoneNumber,
                        PhonenumberConfirmed = patient.PhoneNumberConfirmed,
                        FirstName = patient.FirstName,
                        Surname = patient.Surname,
                        OtherNames = patient.OtherNames,
                        IdNo = patient.IdNo,
                        DateCreated = patient.DateCreated,
                        LocationLatitude = patient.LocationLatitude,
                        LocationLongitude = patient.LocationLongitude,
                        IsOccupied = patient.IsOccupied
                    };
                }
                return Ok(sessions);
            }

            return Ok(await _context.SessionRecords.ToListAsync());
        }

        // id - sessionId
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var session = await _context.SessionRecords.Include(x => x.PatientHealthRecords).FirstOrDefaultAsync(x => x.Id == id);

            if (User.IsInRole("User"))
            {
                var healthOfficer = await _userManager.FindByIdAsync(session.HealthOfficerId);
                session.HealthOfficer = new UserDetails
                {
                    Id = healthOfficer.Id,
                    UserName = healthOfficer.UserName,
                    Email = healthOfficer.Email,
                    EmailConfirmed = healthOfficer.EmailConfirmed,
                    Phonenumber = healthOfficer.PhoneNumber,
                    PhonenumberConfirmed = healthOfficer.PhoneNumberConfirmed,
                    FirstName = healthOfficer.FirstName,
                    Surname = healthOfficer.Surname,
                    OtherNames = healthOfficer.OtherNames,
                    IdNo = healthOfficer.IdNo,
                    DateCreated = healthOfficer.DateCreated,
                    LocationLatitude = healthOfficer.LocationLatitude,
                    LocationLongitude = healthOfficer.LocationLongitude,
                    IsOccupied = healthOfficer.IsOccupied
                };
            }
            if (User.IsInRole("HealthOfficer"))
            {
                var patient = await _userManager.FindByIdAsync(session.PatientId);
                session.Patient = new UserDetails
                {
                    Id = patient.Id,
                    UserName = patient.UserName,
                    Email = patient.Email,
                    EmailConfirmed = patient.EmailConfirmed,
                    Phonenumber = patient.PhoneNumber,
                    PhonenumberConfirmed = patient.PhoneNumberConfirmed,
                    FirstName = patient.FirstName,
                    Surname = patient.Surname,
                    OtherNames = patient.OtherNames,
                    IdNo = patient.IdNo,
                    DateCreated = patient.DateCreated,
                    LocationLatitude = patient.LocationLatitude,
                    LocationLongitude = patient.LocationLongitude,
                    IsOccupied = patient.IsOccupied
                };
            }

            return Ok(session);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PatientHealthRecord value)
        {
            try
            {
                value.Id = Guid.NewGuid().ToString();
                value.Date = DateTime.UtcNow;
                _context.PatientHealthRecords.Add(value);
                await _context.SaveChangesAsync();
                return Ok("Saved successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PatientHealthRecord value)
        {
            try
            {
                _context.Entry(value).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // id - sessionId
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var value = await _context.SessionRecords.FindAsync(id);
                if (value == null) return NotFound("Item not found");
                _context.SessionRecords.Remove(value);
                await _context.SaveChangesAsync();
                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay(SessionRecord value)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound("User not found");
                var profession = await _context.UserProfessions.FindAsync(value.ProfessionId);
                var session = await _context.SessionRecords.Include(x => x.Profession).FirstOrDefaultAsync(x => x.PatientId == user.Id && x.HealthOfficerId == profession.UserId && x.IsComplete == false);
                //if (session != null) return Ok(session.Id);

                value.Id = Guid.NewGuid().ToString();
                value.Amount = value.Profession?.ServiceFee ?? 0;
                value.PatientId = user.Id;
                value.HealthOfficerId = profession.UserId;
                value.ServiceFeePaid = true;
                value.IsComplete = false;
                value.DateStarted = DateTime.UtcNow;

                if (profession.ServiceFee > 0M)
                {
                    // Do Mpesa here
                    //Make payment request using Lipa Na Mpesa

                    string amount = Convert.ToInt32(profession.ServiceFee).ToString();
                    string phonenumber = user.PhoneNumber;
                    if (phonenumber.StartsWith("0"))
                        phonenumber = "254" + phonenumber.Remove(0, 1);
                    if (phonenumber.StartsWith("+254"))
                        phonenumber = "254" + phonenumber.Remove(0, 1);
                    else if (!phonenumber.StartsWith("254"))
                        phonenumber = "254" + phonenumber;

                    string url = BaseApi.MpesaResponseUrl;
                    var response = await requests.LipaNaMpesaOnline(requests.AccessToken(), amount, phonenumber, url);

                    if (string.IsNullOrEmpty(response))
                    {
                        return BadRequest("An error has occured. Please try again. If the error persists, try again later");
                    }

                    var error = JObject.Parse(response)["errorMessage"];
                    if (error != null) return BadRequest(error.ToString());

                    var result = JObject.Parse(response)["ResponseCode"]?.ToString();
                    //if (result == "1032")
                    //{
                    //    return BadRequest("Transaction cancelled");
                    //}
                    if (result != "0")
                    {
                        return BadRequest("Transaction cancelled");
                    }

                    MpesaTransaction transaction = new MpesaTransaction
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = value.HealthOfficerId,
                        TransactionType = "Lipa Na Mpesa Online",
                        Data = response
                    };

                    _context.Add(transaction);
                    var mpesaAccount = await _context.MpesaAccounts.FirstOrDefaultAsync(c => c.UserId == value.HealthOfficerId);
                    if (mpesaAccount == null)
                    {
                        mpesaAccount = new MpesaAccount
                        {
                            UserId = value.HealthOfficerId,
                            AccountBalance = profession.ServiceFee,
                            Date = DateTime.UtcNow
                        };
                        _context.Add(mpesaAccount);
                    }
                    else
                    {
                        mpesaAccount.AccountBalance += profession.ServiceFee;
                        mpesaAccount.Date = DateTime.UtcNow;
                        _context.Update(mpesaAccount);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.SessionRecords.Add(value);

                await _context.SaveChangesAsync();
                return Ok(value.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("close-session/{id}")]
        public async Task<IActionResult> CloseSession(string id)
        {
            try
            {
                var value = await _context.SessionRecords.FindAsync(id);
                if (value == null) return NotFound("Record not found");

                value.IsComplete = true;
                value.DateEnded = DateTime.UtcNow;

                _context.Entry(value).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok("Session closed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sessionId")]
        public async Task<IActionResult> SessionId([FromForm] string professionId, [FromForm] string userId)
        {
            try
            {
                SessionRecord session;
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (User.IsInRole("HealthOfficer"))
                {
                    session = await _context.SessionRecords.FirstOrDefaultAsync(x => x.ProfessionId == professionId && x.HealthOfficerId == currentUser.Id && x.PatientId == userId);
                }
                else
                {
                    session = await _context.SessionRecords.FirstOrDefaultAsync(x => x.ProfessionId == professionId && x.HealthOfficerId == userId && x.PatientId == currentUser.Id);
                }
                return Ok(session.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
