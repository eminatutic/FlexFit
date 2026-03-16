using FlexFit.Data;
using FlexFit.MongoModels.Models;
using MongoDB.Driver;

namespace FlexFit.MongoModels.Repositories
{
    public class RateLimitViolationRepository
    {
        private readonly IMongoCollection<RateLimitViolation> _collection;

        public RateLimitViolationRepository(MongoDbContext context)
        {
            _collection = context.RateLimitViolations;
        }

        public async Task AddAsync(RateLimitViolation violation)
        {
            await _collection.InsertOneAsync(violation);
        }

        public async Task<List<RateLimitViolation>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
