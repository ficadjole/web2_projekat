
using TripService.Models;

namespace TripService.Interfaces
{
    public interface ITripShareRepository
    {
        Task<TripShare?> GetByIdAsync(Guid id);
        Task<TripShare?> GetByTokenAsync(string token);

        Task<IEnumerable<TripShare>> GetByTripIdAsync(Guid tripId);

        Task AddAsync(TripShare tripShare);

        Task DeleteAsync(Guid id);
    }
}
