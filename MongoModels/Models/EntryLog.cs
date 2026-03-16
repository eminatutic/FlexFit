using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexFit.MongoModels.Models
{
    public class EntryLog
    {
        public int MemberId { get; set; }
        public int EmployeeId { get; set; }
        public int FitnessObjectId { get; set; }
        public DateTime Time { get; set; }
        public string CardStatus { get; set; } // Valid / Invalid / DailyPenalty
        public bool Incident { get; set; }
    }
}