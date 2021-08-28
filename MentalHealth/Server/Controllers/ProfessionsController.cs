using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MentalHealth.Server.Data;
using MentalHealth.Shared.Models;
using MentalHealth.Shared.Models.UserAccount;
using MentalHealth.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MentalHealth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ProfessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Professions.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) =>
            Ok(await _context.Professions.FirstOrDefaultAsync(x => x.Id == id));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Profession value)
        {
            try
            {
                value.Id = Guid.NewGuid().ToString();
                _context.Professions.Add(value);
                await _context.SaveChangesAsync();
                return Ok("Save successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Profession value)
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var item = await _context.Professions.FindAsync(id);
                if (item == null) return NotFound("Item not found");

                _context.Professions.Remove(item);
                await _context.SaveChangesAsync();
                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("applications")]
        public async Task<IActionResult> Applications()
        {
            var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.IsApproved == null).ToListAsync();
            var distinctProfessions = new List<UserProfession>();
            foreach (var profession in professions)
            {
                if (distinctProfessions.FirstOrDefault(x => x.ProfessionId == profession.ProfessionId && x.UserId == profession.UserId)?.UserId == profession.UserId) continue;

                var user = await _userManager.FindByIdAsync(profession.UserId);
                profession.User = new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Phonenumber = user.PhoneNumber,
                    PhonenumberConfirmed = user.PhoneNumberConfirmed,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    OtherNames = user.OtherNames,
                    IdNo = user.IdNo,
                    DateCreated = user.DateCreated,
                    LocationLatitude = user.LocationLatitude,
                    LocationLongitude = user.LocationLongitude,
                    IsOccupied = user.IsOccupied
                };
                distinctProfessions.Add(profession);
            }
            return Ok(distinctProfessions);
        }

        [AllowAnonymous]
        [HttpGet("profession/{id}")]
        public async Task<IActionResult> Profession(string id)
        {
            var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.IsApproved == true && x.Profession.Name.ToUpper() == id.ToUpper()).ToListAsync();
            var distinctProfessions = new List<UserProfession>();

            foreach (var profession in professions)
            {
                if (distinctProfessions.FirstOrDefault(x => x.UserId == profession.UserId)?.UserId == profession.UserId) continue;
                var user = await _userManager.FindByIdAsync(profession.UserId);
                if (user.IsOccupied) continue;

                profession.User = new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Phonenumber = user.PhoneNumber,
                    PhonenumberConfirmed = user.PhoneNumberConfirmed,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    OtherNames = user.OtherNames,
                    IdNo = user.IdNo,
                    DateCreated = user.DateCreated,
                    LocationLatitude = user.LocationLatitude,
                    LocationLongitude = user.LocationLongitude,
                    IsOccupied = user.IsOccupied
                };

                distinctProfessions.Add(profession);
            }

            return Ok(distinctProfessions);
        }

        [HttpGet("professionid/{id}")]
        public async Task<IActionResult> GetProfession(string id)
        {
            var profession = await _context.UserProfessions.Include(x => x.Profession).FirstOrDefaultAsync(x => x.Id == id);
            if (profession == null) return NotFound("Profesion not found");
            var user = await _userManager.FindByIdAsync(profession.UserId);
            if (user == null) return NotFound("User not found");
            profession.User = new UserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phonenumber = user.PhoneNumber,
                PhonenumberConfirmed = user.PhoneNumberConfirmed,
                FirstName = user.FirstName,
                Surname = user.Surname,
                OtherNames = user.OtherNames,
                IdNo = user.IdNo,
                DateCreated = user.DateCreated,
                LocationLatitude = user.LocationLatitude,
                LocationLongitude = user.LocationLongitude,
                IsOccupied = user.IsOccupied
            };

            return Ok(profession);
        }

        [HttpGet("recipient/{id}")]
        public async Task<IActionResult> GetProfessionFromSession(string id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var userRole = await _userManager.IsInRoleAsync(currentUser, "User");
            UserProfession profession;
            SessionRecord session;
            if (userRole)
            {
                session = await _context.SessionRecords.Include(x => x.Profession).FirstOrDefaultAsync(x => x.PatientId == currentUser.Id && x.HealthOfficerId == id && x.IsComplete == false);
            }
            else
            {
                session = await _context.SessionRecords.Include(x => x.Profession).FirstOrDefaultAsync(x => x.PatientId == id && x.HealthOfficerId == currentUser.Id && x.IsComplete == false);
            }

            if (session == null) return NotFound("Invalid resquest");

            profession = session.Profession;
            profession.ServiceFeePaid = session.ServiceFeePaid;

            var user = await _userManager.FindByIdAsync(profession.UserId);
            profession.User = new UserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phonenumber = user.PhoneNumber,
                PhonenumberConfirmed = user.PhoneNumberConfirmed,
                FirstName = user.FirstName,
                Surname = user.Surname,
                OtherNames = user.OtherNames,
                IdNo = user.IdNo,
                DateCreated = user.DateCreated,
                LocationLatitude = user.LocationLatitude,
                LocationLongitude = user.LocationLongitude,
                IsOccupied = user.IsOccupied
            };

            return Ok(profession);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromForm] string professionId, [FromForm] decimal serviceFee)
        {
            try
            {
                if (User.IsInRole("Admin")) return BadRequest("Application failed because you are The System Admin");

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound("User not found");
                var profession = new UserProfession
                {
                    UserId = user.Id,
                    ProfessionId = professionId,
                    ServiceFee = serviceFee,
                    IsApproved = null
                };
                if (HttpContext.Request.Form.Files.Any())
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_" + file.FileName;
                        var path = Path.Combine(BaseApi.Storage, fileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        profession.Id = Guid.NewGuid().ToString();
                        profession.DocumentPath = fileName;
                        _context.UserProfessions.Add(profession);
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Application successful and is pending Approval");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //id = profession Id
        [HttpPost("approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve([FromForm] string userId, [FromForm] string professionId)
        {
            try
            {
                var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.ProfessionId == professionId && x.UserId == userId).ToListAsync();
                foreach (var profession in professions)
                {
                    profession.IsApproved = true;
                    _context.Entry(profession).State = EntityState.Modified;
                }

                var user = await _userManager.FindByIdAsync(userId);

                var userRole = await _userManager.IsInRoleAsync(user, "User");
                if (userRole)
                    await _userManager.RemoveFromRoleAsync(user, "User");

                await _userManager.AddToRoleAsync(user, "HealthOfficer");


                await _context.SaveChangesAsync();

                var confirmEmail = $"<b>Congratulations!!!</b> We are happy to inform you that your " +
                    $"application for {professions.FirstOrDefault().Profession.Name} was successful." +
                    $"To see the changes take place, you <em>MUST<em> login again.";
                await _emailSender.SendEmailAsync(user.Email, "Congratulations", confirmEmail);

                return Ok("Proffession approval successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //id = profession Id
        [HttpPost("reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject([FromForm] string userId, [FromForm] string professionId, [FromForm] string reason)
        {
            try
            {
                var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.ProfessionId == professionId && x.UserId == userId).ToListAsync();
                foreach (var profession in professions)
                {
                    profession.IsApproved = false;
                    _context.Entry(profession).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                var confirmEmail = $"We are sorry to inform you that your application for {professions.FirstOrDefault().Profession.Name}" +
                    $" was not successful. <b>{reason}</a>.";

                var user = await _userManager.FindByIdAsync(userId);
                await _emailSender.SendEmailAsync(user.Email, "Application rejected", confirmEmail);

                return Ok("Proffession rejection successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //id = user Id
        [HttpGet("documents/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDocuments(string id)
        {
            //var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.UserId == id).ToListAsync();
            var professions = await _context.UserProfessions.Include(x => x.Profession).Where(x => x.UserId == id && x.IsApproved == null).ToListAsync();
            foreach (var profession in professions)
            {
                var user = await _userManager.FindByIdAsync(profession.UserId);
                profession.User = new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Phonenumber = user.PhoneNumber,
                    PhonenumberConfirmed = user.PhoneNumberConfirmed,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    OtherNames = user.OtherNames,
                    IdNo = user.IdNo,
                    DateCreated = user.DateCreated,
                    LocationLatitude = user.LocationLatitude,
                    LocationLongitude = user.LocationLongitude,
                    IsOccupied = user.IsOccupied
                };
            }

            return Ok(professions);
        }

        //id = document path
        [HttpGet("document/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDocument(string id)
        {
            var file = $"{BaseApi.Storage}/{id}";
            return System.IO.File.Exists(file) ? Ok(new FileStream(file, FileMode.Open)) : NotFound();
        }

    }
}