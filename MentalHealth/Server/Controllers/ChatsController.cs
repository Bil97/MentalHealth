using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentalHealth.Server.Data;
using MentalHealth.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using MentalHealth.Server.Hubs;
using Microsoft.AspNetCore.Identity;
using MentalHealth.Shared.Models.UserAccount;
using Microsoft.AspNetCore.Authorization;

namespace MentalHealth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatsController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        // GET: api/Chats
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetChat()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var chats = await _context.Chats.ToListAsync();
            var distinctChats = new List<Chat>();
            if (user == null || chats == null)
            {
                return NotFound();
            }
            foreach (var chat in chats)
            {
                if (distinctChats.FirstOrDefault(i => i.SessionId == chat.SessionId)?.SessionId == chat.SessionId) continue;

                chat.CurrentUserId = user.Id;
                var senderA = await _userManager.FindByIdAsync(chat.SenderAId);
                chat.SenderA = new UserDetails
                {
                    Id = senderA.Id,
                    UserName = senderA.UserName,
                    Email = senderA.Email,
                    EmailConfirmed = senderA.EmailConfirmed,
                    Phonenumber = senderA.PhoneNumber,
                    PhonenumberConfirmed = senderA.PhoneNumberConfirmed,
                    FirstName = senderA.FirstName,
                    Surname = senderA.Surname,
                    OtherNames = senderA.OtherNames,
                    IdNo = senderA.IdNo,
                    DateCreated = senderA.DateCreated,
                    LocationLatitude = senderA.LocationLatitude,
                    LocationLongitude = senderA.LocationLongitude,
                    IsOccupied = senderA.IsOccupied
                };
                var senderB = await _userManager.FindByIdAsync(chat.SenderBId);
                chat.SenderB = new UserDetails
                {
                    Id = senderB.Id,
                    UserName = senderB.UserName,
                    Email = senderB.Email,
                    EmailConfirmed = senderB.EmailConfirmed,
                    Phonenumber = senderB.PhoneNumber,
                    PhonenumberConfirmed = senderB.PhoneNumberConfirmed,
                    FirstName = senderB.FirstName,
                    Surname = senderB.Surname,
                    OtherNames = senderB.OtherNames,
                    IdNo = senderB.IdNo,
                    DateCreated = senderB.DateCreated,
                    LocationLatitude = senderB.LocationLatitude,
                    LocationLongitude = senderB.LocationLongitude,
                    IsOccupied = senderB.IsOccupied
                };
                distinctChats.Add(chat);
            }

            return Ok(distinctChats.OrderByDescending(i => i.DateSent).ToList());
        }

        // GET: api/Chats/
        [HttpGet("inbox")]
        public async Task<IActionResult> GetMyChat()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var chats = await _context.Chats.Where(i => i.SenderAId == user.Id || i.SenderBId == user.Id).ToListAsync();
            var distinctChats = new List<Chat>();
            if (user == null || chats == null)
            {
                return NotFound();
            }
            foreach (var chat in chats)
            {
                if (distinctChats.FirstOrDefault(i => i.SessionId == chat.SessionId)?.SessionId == chat.SessionId) continue;
                var session = await _context.SessionRecords.FirstOrDefaultAsync(i => i.Id == chat.SessionId);
                if (session.IsComplete) continue;

                chat.CurrentUserId = user.Id;
                var senderA = await _userManager.FindByIdAsync(chat.SenderAId);
                chat.SenderA = new UserDetails
                {
                    Id = senderA.Id,
                    UserName = senderA.UserName,
                    Email = senderA.Email,
                    EmailConfirmed = senderA.EmailConfirmed,
                    Phonenumber = senderA.PhoneNumber,
                    PhonenumberConfirmed = senderA.PhoneNumberConfirmed,
                    FirstName = senderA.FirstName,
                    Surname = senderA.Surname,
                    OtherNames = senderA.OtherNames,
                    IdNo = senderA.IdNo,
                    DateCreated = senderA.DateCreated,
                    LocationLatitude = senderA.LocationLatitude,
                    LocationLongitude = senderA.LocationLongitude,
                    IsOccupied = senderA.IsOccupied
                };
                var senderB = await _userManager.FindByIdAsync(chat.SenderBId);
                chat.SenderB = new UserDetails
                {
                    Id = senderB.Id,
                    UserName = senderB.UserName,
                    Email = senderB.Email,
                    EmailConfirmed = senderB.EmailConfirmed,
                    Phonenumber = senderB.PhoneNumber,
                    PhonenumberConfirmed = senderB.PhoneNumberConfirmed,
                    FirstName = senderB.FirstName,
                    Surname = senderB.Surname,
                    OtherNames = senderB.OtherNames,
                    IdNo = senderB.IdNo,
                    DateCreated = senderB.DateCreated,
                    LocationLatitude = senderB.LocationLatitude,
                    LocationLongitude = senderB.LocationLongitude,
                    IsOccupied = senderB.IsOccupied
                };
                distinctChats.Add(chat);
            }

            return Ok(distinctChats.OrderByDescending(i => i.DateSent).ToList());
        }
        //id= sessionId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChats(string id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var chats = await _context.Chats.Where(i => i.SessionId == id).ToListAsync();

            var distinctChats = new List<Chat>();
            if (chats == null)
            {
                return NotFound();
            }
            foreach (var chat in chats)
            {
                var session = await _context.SessionRecords.FirstOrDefaultAsync(i => i.Id == chat.SessionId);

                chat.SessionRecord = session;
                chat.CurrentUserId = user.Id;
                var senderA = await _userManager.FindByIdAsync(chat.SenderAId);
                chat.SenderA = new UserDetails
                {
                    Id = senderA.Id,
                    UserName = senderA.UserName,
                    Email = senderA.Email,
                    EmailConfirmed = senderA.EmailConfirmed,
                    Phonenumber = senderA.PhoneNumber,
                    PhonenumberConfirmed = senderA.PhoneNumberConfirmed,
                    FirstName = senderA.FirstName,
                    Surname = senderA.Surname,
                    OtherNames = senderA.OtherNames,
                    IdNo = senderA.IdNo,
                    DateCreated = senderA.DateCreated,
                    LocationLatitude = senderA.LocationLatitude,
                    LocationLongitude = senderA.LocationLongitude,
                    IsOccupied = senderA.IsOccupied
                };
                var senderB = await _userManager.FindByIdAsync(chat.SenderBId);
                chat.SenderB = new UserDetails
                {
                    Id = senderB.Id,
                    UserName = senderB.UserName,
                    Email = senderB.Email,
                    EmailConfirmed = senderB.EmailConfirmed,
                    Phonenumber = senderB.PhoneNumber,
                    PhonenumberConfirmed = senderB.PhoneNumberConfirmed,
                    FirstName = senderB.FirstName,
                    Surname = senderB.Surname,
                    OtherNames = senderB.OtherNames,
                    IdNo = senderB.IdNo,
                    DateCreated = senderB.DateCreated,
                    LocationLatitude = senderB.LocationLatitude,
                    LocationLongitude = senderB.LocationLongitude,
                    IsOccupied = senderB.IsOccupied
                };
                if (chat.SenderId == chat.SenderAId) chat.Sender = chat.SenderA;
                else chat.Sender = chat.SenderB;
                distinctChats.Add(chat);
            }

            return Ok(distinctChats.OrderByDescending(i => i.DateSent).ToList());
        }

        // PUT: api/Chats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutChat(Chat chat)
        {
            try
            {
            _context.Entry(chat).State = EntityState.Modified;
               await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "message");
                return Ok("Message sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Chats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostChat(Chat chat)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (string.IsNullOrEmpty(chat.SenderAId))
            {
                chat.SenderAId = user.Id;
            }

            chat.DateSent = DateTime.UtcNow;
            chat.Id = Guid.NewGuid().ToString();
            chat.SenderId = user.Id;
            chat.IsRead = false;
            _context.Chats.Add(chat);
            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "message");
                return Ok("Message sent");
            }
            catch (DbUpdateException)
            {
                if (ChatExists(chat.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Chats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(string id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "message");
            return NoContent();
        }
        //id=sessionId
        // DELETE: api/Chats/5
        [HttpDelete("clear/{id}")]
        public async Task<IActionResult> ClearChat(string id)
        {
            var chats = await _context.Chats.Where(i => i.SessionId == id).ToListAsync();
            if (chats == null)
            {
                return NotFound();
            }

            _context.Chats.RemoveRange(chats);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "message");
            return NoContent();
        }

        private bool ChatExists(string id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
