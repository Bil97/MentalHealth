using MentalHealth.Models.UserAccount;
using System;

namespace MentalHealth.Models
{
    public class Chat
    {
        public string Id { get; set; }
        public string CurrentUserId { get; set; }
        public string ConversationId { get; set; }
        public UserDetails SenderA { get; set; }
        public string SenderAId { get; set; }
        public UserDetails SenderB { get; set; }
        public string SenderBId { get; set; }
        public UserDetails Sender { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset DateSent { get; set; }
        public SessionRecord SessionRecord { get; set; }
        public string SessionId { get; set; }
    }
}
