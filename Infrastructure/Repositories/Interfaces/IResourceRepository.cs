using FlexFit.Domain.Models;

namespace FlexFit.Infrastructure.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<Resource> GetByIdAsync(int id);
        Task<IEnumerable<Resource>> GetAllAsync();
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(Resource resource);

        Task<Resource> CreateResourceAsync(FlexFit.Application.DTOs.CreateResourceDto dto);
    }
}