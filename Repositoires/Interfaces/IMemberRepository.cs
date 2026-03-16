using FlexFit.Models;

namespace FlexFit.Repositoires.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(int id);
        Task<IEnumerable<Member>> GetAllAsync();
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Member member);
    }
}