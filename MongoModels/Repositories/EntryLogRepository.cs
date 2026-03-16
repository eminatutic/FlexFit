using FlexFit.Data;
using FlexFit.MongoModels.Models;
using MongoDB.Driver;

namespace FlexFit.MongoModels.Repositories
{
    public class EntryLogRepository
    {
        private readonly IMongoCollection<EntryLog> _collection;

        public EntryLogRepository(MongoDbContext context)
        {
            _collection = context.EntryLogs;
        }

        public async Task AddAsync(EntryLog log)
        {
            await _collection.InsertOneAsync(log);
        }

        public async Task<List<EntryLog>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
