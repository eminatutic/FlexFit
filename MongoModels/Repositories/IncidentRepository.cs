using FlexFit.Data;
using FlexFit.MongoModels.Models;
using MongoDB.Driver;

namespace FlexFit.MongoModels.Repositories
{
    public class IncidentRepository
    {
        private readonly IMongoCollection<Incident> _collection;

        public IncidentRepository(MongoDbContext context)
        {
            _collection = context.Incidents;
        }

        public async Task AddAsync(Incident incident)
        {
            await _collection.InsertOneAsync(incident);
        }

        public async Task<List<Incident>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
