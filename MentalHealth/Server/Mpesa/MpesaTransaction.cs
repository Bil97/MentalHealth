using MentalHealth.Shared.Models.UserAccount;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Server.Mpesa
{
    public class MpesaTransaction
    {
        public string Id { get; set; }

        [NotMapped]
        public UserDetails User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        public string Data { get; set; }

        [Display(Name = "Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
