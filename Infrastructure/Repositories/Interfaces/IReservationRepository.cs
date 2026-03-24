using FlexFit.Domain.Models;

namespace FlexFit.Infrastructure.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<IEnumerable<Reservation>> FindAsync(System.Linq.Expressions.Expression<Func<Reservation, bool>> predicate);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task<(bool isSuccess, string message)> BookResourceAsync(FlexFit.Application.DTOs.ReservationDto dto);
    }
}