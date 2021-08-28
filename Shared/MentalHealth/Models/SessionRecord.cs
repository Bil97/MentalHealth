using MentalHealth.Models.UserAccount;
using System;
using System.Collections.Generic;

namespace MentalHealth.Models
{
    public class SessionRecord
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public UserDetails Patient { get; set; }
        public string PatientId { get; set; }
        public UserDetails HealthOfficer { get; set; }
        public string HealthOfficerId { get; set; }

        public UserProfession Profession { get; set; }
        public string ProfessionId { get; set; }

        public bool ServiceFeePaid { get; set; }

        public DateTimeOffset DateStarted { get; set; }
        public DateTimeOffset DateEnded { get; set; }

        public bool IsComplete { get; set; }

        public ICollection<PatientHealthRecord> PatientHealthRecords { get; set; }
    }
}