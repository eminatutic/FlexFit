using FlexFit.Infrastructure.Data;
using FlexFit.Domain.MongoModels.Models;
using MongoDB.Driver;

namespace FlexFit.Domain.MongoModels.Repositories
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
