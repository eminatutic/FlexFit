using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexFit.MongoModels.Models
{
    public class Login
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserId { get; set; } = string.Empty;   // ID člana ili zaposlenog
        public string Email { get; set; } = string.Empty;    // email korisnika
        public string Role { get; set; } = "Member";        // "Member" ili "Employee"
        public DateTime Time { get; set; } = DateTime.UtcNow; // vreme logina
    }
}