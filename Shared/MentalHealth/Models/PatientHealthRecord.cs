using System;

namespace MentalHealth.Models
{
    public class PatientHealthRecord
    {
        public string Id { get; set; }
        public SessionRecord SessionRecord { get; set; }
        public string SessionRecordId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string HealthRecord { get; set; }
    }
}