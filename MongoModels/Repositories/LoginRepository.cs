using FlexFit.Data;
using FlexFit.MongoModels.Models;
using MongoDB.Driver;

namespace FlexFit.MongoModels.Repositories
{
    public class LoginRepository
    {
        private readonly IMongoCollection<Login> _collection;

        public LoginRepository(MongoDbContext context)
        {
            _collection = context.Login;
        }

        // Dodavanje novog logina
        public async Task AddAsync(Login log)
        {
            await _collection.InsertOneAsync(log);
        }

        // Dohvatanje svih logina
        public async Task<List<Login>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        // Dohvatanje logina po korisniku
        public async Task<List<Login>> GetByUserIdAsync(string userId)
        {
            return await _collection.Find(l => l.UserId == userId).ToListAsync();
        }

        // Dohvatanje logina po emailu
        public async Task<List<Login>> GetByEmailAsync(string email)
        {
            return await _collection.Find(l => l.Email == email).ToListAsync();
        }

        // Dohvatanje logina u određenom vremenskom periodu
        public async Task<List<Login>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _collection.Find(l => l.Time >= from && l.Time <= to).ToListAsync();
        }

        // Opcionalno: Dohvatanje poslednjeg logina korisnika
        public async Task<Login?> GetLastLoginAsync(string userId)
        {
            return await _collection
                         .Find(l => l.UserId == userId)
                         .SortByDescending(l => l.Time)
                         .FirstOrDefaultAsync();
        }
    }
}