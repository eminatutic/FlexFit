using FlexFit.Domain.Models;

namespace FlexFit.Infrastructure.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(int id);
        Task<Member> GetByEmailAsync(string email); 
        Task<IEnumerable<Member>> GetAllAsync();
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Member member);
    }
}