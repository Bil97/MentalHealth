using System;

namespace MentalHealth.Models.UserAccount
{
    public class UserDetails
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string Phonenumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }

        public string FullName => $"{FirstName} {Surname} {OtherNames}";
        public string IdNo { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string HealthStatus { get; set; }

        public string Profession { get; set; }

        public bool IsOccupied { get; set; }

        public string ImagePath { get; set; }
    }
}