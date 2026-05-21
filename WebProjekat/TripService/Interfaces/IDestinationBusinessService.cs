using TripService.Interfaces.DTOs.Destination;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IDestinationBusinessService
    {
        Task<Result<DestinationDto>> CreateAsync(CreateDestinationDto dto, Guid userId);
        Task<Result<DestinationDto>> GetByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<DestinationDto>>> GetByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<DestinationDto>> UpdateAsync(Guid id, UpdateDestinationDto dto, Guid userId);
        Task<Result> DeleteAsync(Guid id, Guid userId);
    }
}
