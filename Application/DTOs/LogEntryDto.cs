using System.Text.Json.Serialization;

namespace FlexFit.Application.DTOs
{
    public class LogEntryDto
    {
        public int MemberId { get; set; }
        public int FitnessObjectId { get; set; }
        public string CardNumber { get; set; }
        public string CardStatus { get; set; } // e.g., "Active", "Expired", "Invalid"
        public string CardType { get; set; } // "Subscription" or "Daily" (or "Dnevna"/"Pretplatna")
        
        [JsonPropertyName("employeeId")]
        public int EmployeeId { get; set; }
    }
}
