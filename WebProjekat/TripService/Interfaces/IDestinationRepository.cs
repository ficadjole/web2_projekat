using Common.Interfaces;
using TripService.Models;

namespace TripService.Interfaces
{
    public interface IDestinationRepository : IRepository<Destination>
    {
        Task<IEnumerable<Destination>> GetByTripIdAsync(Guid tripId);
        Task<Destination?> GetByIdWithActivitiesAsync(Guid id);
    }
}
