using Common.Interfaces;
using TripService.Models;

namespace TripService.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetByDestinationIdAsync(Guid destinationId);
        Task<IEnumerable<Activity>> GetByDateAsync(Guid tripId, DateTime date); // za calendar view
    }
}
