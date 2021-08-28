using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Shared.Models.UserAccount
{
    public class UserProfession
    {
        public string Id { get; set; }
        [NotMapped]
        public UserDetails User { get; set; }

        public string UserId { get; set; }
        public Profession Profession { get; set; }

        [ForeignKey(nameof(Profession))]
        public string ProfessionId { get; set; }

        public bool? IsApproved { get; set; }

        public string DocumentPath { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0)]
        public decimal ServiceFee { get; set; }
        [NotMapped]
        public bool ServiceFeePaid { get; set; }
    }
}