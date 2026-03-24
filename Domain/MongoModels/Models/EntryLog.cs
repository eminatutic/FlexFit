using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexFit.Domain.MongoModels.Models
{
    public class EntryLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 

        public int MemberId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Time { get; set; }
        public string CardStatus { get; set; }
        public string CardType { get; set; }
        public string? Incident { get; set; }
    }
}