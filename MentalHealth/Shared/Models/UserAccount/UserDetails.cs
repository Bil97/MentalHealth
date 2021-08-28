using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MentalHealth.Shared.Models.UserAccount
{
    public class UserDetails
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Email confirmed")] public bool EmailConfirmed { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        [Display(Name = "Phonenumber confirmed")]
        public bool PhonenumberConfirmed { get; set; }
        [Required]
        [Display(Name = "Surname")] public string Surname { get; set; }
        [Required]
        [Display(Name = "First name")] public string FirstName { get; set; }
        [Display(Name = "Other names")] public string OtherNames { get; set; }
        public string FullName => $"{FirstName} {Surname} {OtherNames}";
        [Required]
        [Display(Name = "National ID number or Passport number")]
        public string IdNo { get; set; }
        [Display(Name = "Date created")] public DateTimeOffset DateCreated { get; set; }
        [Required] [DefaultValue(0)] public double LocationLatitude { get; set; }
        [Required] [DefaultValue(0)] public double LocationLongitude { get; set; }
        public string Profession { get; set; }
        public bool IsOccupied { get; set; }
        public string ImagePath { get; set; }
    }
}