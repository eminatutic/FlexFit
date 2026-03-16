using FlexFit.Models;

namespace FlexFit.Repositoires.Interfaces
{
    public interface IMembershipCardRepository
    {
        Task<MembershipCard> GetByIdAsync(int id);
        Task<IEnumerable<MembershipCard>> GetAllAsync();
        Task AddAsync(MembershipCard card);
        Task UpdateAsync(MembershipCard card);
        Task DeleteAsync(MembershipCard card);
    }
}