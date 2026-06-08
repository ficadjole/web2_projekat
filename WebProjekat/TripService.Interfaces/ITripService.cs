using Microsoft.ServiceFabric.Services.Remoting;
using TripService.Interfaces.DTOs.Trip;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface ITripService : IService
    {
        Task<Result<TripDto>> CreateTripAsync(CreateTripDto dto, Guid userId);
        Task<Result<TripDto>> GetTripByIdAsync(Guid id, Guid userId, bool isAdmin);
        Task<Result<TripDetailsDto>> GetTripWithDetailsAsync(Guid id, Guid userId, bool isAdmin);
        Task<Result<IEnumerable<TripDto>>> GetAllTripsByUserAsync(Guid userId);
        Task<Result<TripDto>> UpdateTripAsync(Guid id, UpdateTripDto dto, Guid userId);
        Task<Result> DeleteTripAsync(Guid id, Guid userId, bool isAdmin);
        Task<Result> DeleteAllByUser(Guid userId, bool isAdmin);
        Task<Result<IEnumerable<TripDto>>> GetAllAsync();
    }
}
