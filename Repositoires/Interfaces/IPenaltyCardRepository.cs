using FlexFit.Models;

namespace FlexFit.Repositoires.Interfaces
{
    public interface IPenaltyCardRepository
    {
        Task<PenaltyCard> GetByIdAsync(int id);
        Task<IEnumerable<PenaltyCard>> GetAllAsync();
        Task AddAsync(PenaltyCard penaltyCard);
        Task UpdateAsync(PenaltyCard penaltyCard);
        Task DeleteAsync(PenaltyCard penaltyCard);
    }
}