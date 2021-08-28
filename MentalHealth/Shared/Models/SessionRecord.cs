using MentalHealth.Shared.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Shared.Models
{
    public class SessionRecord
    {
        public string Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [NotMapped]
        public UserDetails Patient { get; set; }
        public string PatientId { get; set; }
        [NotMapped]
        public UserDetails HealthOfficer { get; set; }
        public string HealthOfficerId { get; set; }

        public UserProfession Profession { get; set; }
        [ForeignKey(nameof(Profession))]
        public string ProfessionId { get; set; }

        [DefaultValue(false)]
        public bool ServiceFeePaid { get; set; }

        public DateTimeOffset DateStarted { get; set; }
        public DateTimeOffset DateEnded { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }

        public ICollection<PatientHealthRecord> PatientHealthRecords { get; set; }
    }
}