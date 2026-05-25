
using TripService.Interfaces.DTOs.TripShare;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    internal interface ITripShareBusinessService
    {
        Task<Result<TripShareDto>> CreateShareAsync(CreateTripShareDto dto, Guid userId);
        Task<Result<SharedTripDto>> GetSharedTripAsync(string token);
        Task<Result<IEnumerable<TripShareDto>>> GetSharesByTripIdAsync(Guid tripId, Guid userId);
        Task<Result> RevokeShareAsync(Guid id, Guid userId);
    }
}
