using FlexFit.Infrastructure.Data;
using FlexFit.Domain.Models;
using FlexFit.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlexFit.Infrastructure.Repositories
{
    public class MembershipCardRepository : IMembershipCardRepository
    {
        private readonly AppDbContext _context;

        public MembershipCardRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MembershipCard> GetByCardNumberAsync(string cardNumber) =>
            await _context.MembershipCards
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        public async Task<MembershipCard> GetByIdAsync(int id) =>
            await _context.MembershipCards
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<MembershipCard>> GetAllAsync() =>
            await _context.MembershipCards.ToListAsync();

        public async Task AddAsync(MembershipCard card)
        {
            await _context.MembershipCards.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MembershipCard card)
        {
            _context.MembershipCards.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MembershipCard card)
        {
            _context.MembershipCards.Remove(card);
            await _context.SaveChangesAsync();
        }
    }
}