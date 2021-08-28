using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MentalHealth.Server.Data;
using MentalHealth.Shared.Models.UserAccount;
using MentalHealth.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public ApplicationUsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetApplicationUser()
        {
            var applicationUsers = await _userManager.Users.ToListAsync();

            return Ok(applicationUsers.Select(applicationUser => new UserDetails
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Phonenumber = applicationUser.PhoneNumber,
                PhonenumberConfirmed = applicationUser.PhoneNumberConfirmed,
                FirstName = applicationUser.FirstName,
                Surname = applicationUser.Surname,
                OtherNames = applicationUser.OtherNames,
                IdNo = applicationUser.IdNo,
                DateCreated = applicationUser.DateCreated,
                LocationLatitude = applicationUser.LocationLatitude,
                LocationLongitude = applicationUser.LocationLongitude,
                IsOccupied = applicationUser.IsOccupied,
                ImagePath = applicationUser.ImagePath
            }).ToList());
        }

        // GET: api/ApplicationUsers/UserName
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplicationUser(string id)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == id);

            if (applicationUser == null)
            {
                return NotFound("User not found");
            }

            var userDetails = new UserDetails
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Phonenumber = applicationUser.PhoneNumber,
                PhonenumberConfirmed = applicationUser.PhoneNumberConfirmed,
                FirstName = applicationUser.FirstName,
                Surname = applicationUser.Surname,
                OtherNames = applicationUser.OtherNames,
                IdNo = applicationUser.IdNo,
                DateCreated = applicationUser.DateCreated,
                LocationLatitude = applicationUser.LocationLatitude,
                LocationLongitude = applicationUser.LocationLongitude,
                IsOccupied = applicationUser.IsOccupied,
                ImagePath = applicationUser.ImagePath
            };

            return Ok(userDetails);
        }

        // GET: api/ApplicationUsers/UserId
        [HttpGet("userId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplicationUserById(string id)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (applicationUser == null)
            {
                return NotFound("User not found");
            }

            var userDetails = new UserDetails
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Phonenumber = applicationUser.PhoneNumber,
                PhonenumberConfirmed = applicationUser.PhoneNumberConfirmed,
                FirstName = applicationUser.FirstName,
                Surname = applicationUser.Surname,
                OtherNames = applicationUser.OtherNames,
                IdNo = applicationUser.IdNo,
                DateCreated = applicationUser.DateCreated,
                LocationLatitude = applicationUser.LocationLatitude,
                LocationLongitude = applicationUser.LocationLongitude,
                IsOccupied = applicationUser.IsOccupied,
                ImagePath = applicationUser.ImagePath
            };

            return Ok(userDetails);
        }

        // PUT: api/ApplicationUsers/update-account/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update-account/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateApplicationUser(string id, UserDetails userDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstOrDefault().Value);
            }
            var applicationUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (id != applicationUser?.Id)
            {
                return BadRequest("Invalid request");
            }

            if (applicationUser == null) return NotFound("User not found");
            applicationUser.UserName = userDetails.UserName;
            applicationUser.Email = userDetails.Email;
            applicationUser.EmailConfirmed = userDetails.EmailConfirmed;
            applicationUser.PhoneNumber = userDetails.Phonenumber;
            applicationUser.PhoneNumberConfirmed = userDetails.PhonenumberConfirmed;
            applicationUser.FirstName = userDetails.FirstName;
            applicationUser.Surname = userDetails.Surname;
            applicationUser.OtherNames = userDetails.OtherNames;
            applicationUser.IdNo = userDetails.IdNo;
            applicationUser.ImagePath = userDetails.ImagePath;

            try
            {
                await _userManager.UpdateAsync(applicationUser);
                return Ok("Account update successful");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound("User does not exist");
                }

                throw;
            }
        }

        // POST: api/ApplicationUsers/create-account
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateApplicationUser([FromBody] Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstOrDefault().Value);
            }

            var applicationUser = new ApplicationUser
            {
                FirstName = register.FirstName,
                Surname = register.Surname,
                OtherNames = register.OtherNames,
                UserName = register.Email,
                Email = register.Email,
                PhoneNumber = $"{Convert.ToInt64(register.PhoneNumber)}",
                DateCreated = DateTime.UtcNow,
                LocationLatitude = register.LocationLatitude,
                LocationLongitude = register.LocationLongitude,
                IdNo = register.IdNo,
                IsOccupied = false,
            };


            var result = await _userManager.CreateAsync(applicationUser, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(register.Email);
                // Add the first user to the admins role
                if (_userManager.Users.Count() <= 1)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                var callbackUrl = $"{Request.Scheme}://{Request.Host}/confirm-email/{user.Id}/{user.SecurityStamp}";

                var confirmEmail =
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                try
                {
                    await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", confirmEmail);
                }
                catch (SmtpFailedRecipientException)
                {
                    await _userManager.DeleteAsync(user);
                    await _context.SaveChangesAsync();
                    return NotFound("Invalid email address");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }

                await _signInManager.PasswordSignInAsync(register.Email, register.Password, true, false);

                return Ok(new JwtSecurityTokenHandler().WriteToken(await Token(register.Email)));
            }
        }

        // POST: api/ApplicationUsers/confirm-email
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail confirmEmail)
        {
            if (confirmEmail.Id == null || confirmEmail.Code == null)
            {
                return NotFound($"Unable to load user with ID '{confirmEmail.Id}'.");
            }

            var user = await _userManager.FindByIdAsync(confirmEmail.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{confirmEmail.Id}'.");
            }

            if (user.SecurityStamp == confirmEmail.Code)
            {
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                return Ok("Thank you for confirming your email. Login in to access your account");
            }
            else
            {
                return BadRequest("Error confirming your email.");
            }
        }

        // POST: api/ApplicationUsers/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstOrDefault().Value);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

            if (!result.Succeeded)
            {
                return BadRequest("Incorrect username or password");
            }

            return Ok(new JwtSecurityTokenHandler().WriteToken(await Token(login.Email)));
        }

        private async Task<JwtSecurityToken> Token(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: credentials
            );
            return token;
        }


        // Post: api/ApplicationUsers/logout
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
                if (user == null)
                {
                    return NotFound("Email does not exist");
                }

                var callbackUrl = $"{Request.Scheme}://{Request.Host}/user-account/reset-password/{user.Id}/{user.SecurityStamp}";
                var confirmEmail =
                    $"Reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                await _emailSender.SendEmailAsync(forgotPassword.Email, "Reset your password", confirmEmail);
                return Ok("Check your email for a link to reset your password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            if (resetPassword.Id == null || resetPassword.Code == null)
            {
                return NotFound($"Unable to load user with ID '{resetPassword.Id}'.");
            }

            var user = await _userManager.FindByIdAsync(resetPassword.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{resetPassword.Id}'.");
            }

            if (user.SecurityStamp == resetPassword.Code)
            {
                try
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _context.SaveChangesAsync();

                    var result = await _userManager.AddPasswordAsync(user, resetPassword.NewPassword);
                    if (result.Succeeded)
                    {
                        return Ok("Password reset successful. Login in to access your account");
                    }
                    else
                    {
                        return BadRequest(result.Errors.FirstOrDefault()?.Description);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Error resetting your password.");
            }
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound("An error occured when updating your password");
            }

            try
            {
                var result =
                    await _userManager.ChangePasswordAsync(user, changePassword.OldPassword,
                        changePassword.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }
                else
                {
                    return Ok("Password change successful.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApplicationUsers/delete-account/5
        [HttpDelete("delete-account/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (id != applicationUser.Id && !User.IsInRole("Admin")) return BadRequest("Invalid request");
            try
            {
                await _userManager.DeleteAsync(applicationUser);
                return Ok("Account delete successful");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound("User does not exist");
                }

                throw;
            }

        }

        [HttpGet("image/{id}")]
        public IActionResult GetImage(string id)
        {
            var file = $"{BaseApi.Storage}/{id}";
            return System.IO.File.Exists(file) ? Ok(new FileStream(file, FileMode.Open)) : NotFound();
        }

        private bool ApplicationUserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}