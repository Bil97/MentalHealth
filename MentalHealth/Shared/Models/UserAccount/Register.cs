using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MentalHealth.Shared.Models.UserAccount
{
    public class Register
    {
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Other names")]
        public string OtherNames { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        // [RegularExpression(@"^[0-9]{9,10}$", ErrorMessage = "Phone number is invalid")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string IdNo { get; set; }
        [Required]
        [DefaultValue(0)]
        public double LocationLatitude { get; set; }
        [DefaultValue(0)]
        public double LocationLongitude { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
