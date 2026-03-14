using MongoDB.Driver;
using FlexFit.MongoModels;

namespace FlexFit.Services
{
    public class MongoService
    {
        private readonly IMongoDatabase _database;

        public MongoService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("flexfit_logs");
        }

        public IMongoCollection<EntryLog> EntryLogs =>
            _database.GetCollection<EntryLog>("entry_logs");
    }
}