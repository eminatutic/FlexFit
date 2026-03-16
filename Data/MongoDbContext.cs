using MongoDB.Driver;
using FlexFit.MongoModels.Models;

namespace FlexFit.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("flexfit_logs");
        }

        public IMongoCollection<EntryLog> EntryLogs =>
            _database.GetCollection<EntryLog>("entry_logs");

        public IMongoCollection<Incident> Incidents =>
            _database.GetCollection<Incident>("incidents");

        public IMongoCollection<RateLimitViolation> RateLimitViolations =>
            _database.GetCollection<RateLimitViolation>("rate_limit_violations");
    }
}