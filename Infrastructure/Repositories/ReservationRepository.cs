using FlexFit.Infrastructure.Data;
using FlexFit.Domain.Models;
using FlexFit.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FlexFit.Application.DTOs;
using FlexFit.Domain.Interfaces.Repositories;

namespace FlexFit.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemberGraphRepository _graphRepo;

        public ReservationRepository(AppDbContext context, IMemberGraphRepository graphRepo)
        {
            _context = context;
            _graphRepo = graphRepo;
        }

        public async Task<Reservation> GetByIdAsync(int id) =>
            await _context.Reservations
                .Include(r => r.Member)
                .Include(r => r.Resource)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Reservation>> GetAllAsync() =>
            await _context.Reservations
                .Include(r => r.Member)
                .Include(r => r.Resource)
                .ToListAsync();

        public async Task<IEnumerable<Reservation>> FindAsync(System.Linq.Expressions.Expression<Func<Reservation, bool>> predicate) =>
            await _context.Reservations
                .Include(r => r.Member)
                .Include(r => r.Resource)
                .Where(predicate)
                .ToListAsync();

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool isSuccess, string message)> BookResourceAsync(ReservationDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
                return (false, "PoÄ etno vreme mora biti pre krajnjeg.");

            var alreadyBooked = await _context.Reservations.AnyAsync(r => 
                r.MemberId == dto.MemberId && r.ResourceId == dto.ResourceId && r.StartTime == dto.StartTime && r.Status != ReservationStatus.NoShow);

            if (alreadyBooked)
            {
                return (false, "VeÄ‡ ste uspeÅ¡no zakazali ovaj termin. Nije moguÄ‡e zakazati isti termin viÅ¡e puta.");
            }

            var resource = await _context.Resources.FindAsync(dto.ResourceId);
            int maxCapacity = (resource != null && resource.Type == ResourceType.GrupnaSala) ? 10 : 5;

            var concurrent = await _context.Reservations
                .Where(r => 
                    r.ResourceId == dto.ResourceId &&
                    r.Status != ReservationStatus.NoShow &&
                    ((dto.StartTime >= r.StartTime && dto.StartTime < r.EndTime) || 
                     (dto.EndTime > r.StartTime && dto.EndTime <= r.EndTime) || 
                     (dto.StartTime <= r.StartTime && dto.EndTime >= r.EndTime))
                ).ToListAsync();

            if (concurrent.Count >= maxCapacity)
            {
                return (false, $"Maksimalan broj osoba ({maxCapacity}) za ovaj termin je veÄ‡ popunjen.");
            }

            var reservation = new Reservation
            {
                MemberId = dto.MemberId,
                ResourceId = dto.ResourceId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = ReservationStatus.Reserved
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            // Sync to Neo4j (Simplified: IDs only)
            await _graphRepo.RecordReservationAsync(dto.MemberId.ToString(), dto.ResourceId);
            
            return (true, "Rezervacija uspeÅ¡na.");
        }
    }
}