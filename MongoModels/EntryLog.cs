using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexFit.MongoModels
{
    public class EntryLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int EmployeeId { get; set; }

        public int MemberId { get; set; }

        public int FitnessObjectId { get; set; }

        public DateTime EntryTime { get; set; }

        public string CardStatus { get; set; }

        public bool Incident { get; set; }

        public string Description { get; set; }
    }
}