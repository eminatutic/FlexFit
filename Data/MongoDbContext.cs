using MongoDB.Driver;
using FlexFit.MongoModels;

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
    }
}