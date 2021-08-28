using MentalHealth.Models;

namespace MentalHealth.Models.UserAccount
{
    public class UserProfession
    {
        public string Id { get; set; }

        public UserDetails User { get; set; }

        public string UserId { get; set; }
        public Profession Profession { get; set; }

        public string ProfessionId { get; set; }

        public bool? IsApproved { get; set; }

        public string DocumentPath { get; set; }

        public bool ServiceFeePaid { get; set; }

        public decimal ServiceFee { get; set; }
    }
}