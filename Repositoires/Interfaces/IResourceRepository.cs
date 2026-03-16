using FlexFit.Models;

namespace FlexFit.Repositoires.Interfaces
{
    public interface IResourceRepository
    {
        Task<Resource> GetByIdAsync(int id);
        Task<IEnumerable<Resource>> GetAllAsync();
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(Resource resource);
    }
}