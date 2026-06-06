using Common.Interfaces;
using TripService.Models;
namespace TripService.Interfaces
{
    public interface ITripRepository : IRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetAllByUserIdAsync(Guid userId);
        Task<Trip?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Trip>> GetAllAsync();
    }
}
