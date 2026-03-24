using System.Collections.Generic;
using System.Threading.Tasks;
using FlexFit.Application.DTOs;

namespace FlexFit.Infrastructure.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        Task<IEnumerable<TimeSlotResultDto>> GetTimeSlotsAsync(int resourceId);
        Task<int> CreateTimeSlotAsync(TimeSlotDto dto);
        Task<bool> DeleteTimeSlotAsync(int id);
    }
}
