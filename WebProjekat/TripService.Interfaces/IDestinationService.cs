using Microsoft.ServiceFabric.Services.Remoting;
using TripService.Interfaces.DTOs.Destination;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IDestinationService : IService
    {
        Task<Result<DestinationDto>> CreateDestinationAsync(CreateDestinationDto dto, Guid userId);
        Task<Result<DestinationDto>> GetDestinationByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<DestinationDto>>> GetDestinationsByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<DestinationDto>> UpdateDestinationAsync(Guid id, UpdateDestinationDto dto, Guid userId);
        Task<Result> DeleteDestinationAsync(Guid id, Guid userId);
    }
}
