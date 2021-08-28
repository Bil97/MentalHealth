using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MentalHealth.Shared.Models.UserAccount
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string Surname { get; set; }

        [Required] public string FirstName { get; set; }

        public string OtherNames { get; set; }

        [Required] public string IdNo { get; set; }
        [Required] [DefaultValue(0)] public double LocationLatitude { get; set; }
        [Required] [DefaultValue(0)] public double LocationLongitude { get; set; }
        [Required] public bool IsOccupied { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public string ImagePath { get; set; }

    }
}