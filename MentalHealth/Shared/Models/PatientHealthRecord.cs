using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealth.Shared.Models
{
    public class PatientHealthRecord
    {
        public string Id { get; set; }
        public SessionRecord SessionRecord { get; set; }
        [ForeignKey(nameof(SessionRecord))]
        public string SessionRecordId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string HealthRecord { get; set; }
    }
}