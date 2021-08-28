using MentalHealth.Shared.Models.UserAccount;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Server.Mpesa
{
    public class MpesaAccount
    {
        [Key]
        public string UserId { get; set; }

        [NotMapped]
        public UserDetails ApplicationUser { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
