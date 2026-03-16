using FlexFit.Models;

namespace FlexFit.Repositoires.Interfaces
{
    public interface IPenaltyPointRepository
    {
        Task<PenaltyPoint> GetByIdAsync(int id);
        Task<IEnumerable<PenaltyPoint>> GetAllAsync();
        Task AddAsync(PenaltyPoint point);
        Task UpdateAsync(PenaltyPoint point);
        Task DeleteAsync(PenaltyPoint point);
    }
}
