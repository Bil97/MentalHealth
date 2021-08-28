using MentalHealth.Shared.Models.UserAccount;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Shared.Models
{
    public class Chat
    {
        public string Id { get; set; }
        [NotMapped]
        public string CurrentUserId { get; set; }
        [NotMapped]
        public SessionRecord SessionRecord { get; set; }

        public string SessionId { get; set; }
        [NotMapped]
        public UserDetails SenderA { get; set; }
        public string SenderAId { get; set; }
        [NotMapped]
        public UserDetails SenderB { get; set; }
        public string SenderBId { get; set; }
        [NotMapped]
        public UserDetails Sender { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset DateSent { get; set; }
    }
}
