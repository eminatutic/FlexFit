using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexFit.MongoModels.Models
{
    public class RateLimitViolation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IpAddress { get; set; }    // IP korisnika
        public string Route { get; set; }        // Ruta koju je pokušao
        public DateTime Timestamp { get; set; }  // Vreme prekoračenja
    }
}