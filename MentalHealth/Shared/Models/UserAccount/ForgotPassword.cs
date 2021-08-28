using System.ComponentModel.DataAnnotations;

namespace MentalHealth.Shared.Models.UserAccount
{
    public class ForgotPassword
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
