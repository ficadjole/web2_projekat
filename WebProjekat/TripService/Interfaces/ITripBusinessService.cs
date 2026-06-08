using TripService.Interfaces.DTOs.Trip;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface ITripBusinessService
    {
        Task<Result<TripDto>> CreateAsync(CreateTripDto dto, Guid userId);
        Task<Result<TripDto>> GetByIdAsync(Guid id, Guid userId, bool isAdmin = false);
        Task<Result<TripDetailsDto>> GetWithDetailsAsync(Guid id, Guid userId, bool isAdmin = false);
        Task<Result<IEnumerable<TripDto>>> GetAllByUserAsync(Guid userId);

        Task<Result<IEnumerable<TripDto>>> GetAllAsync();
        Task<Result<TripDto>> UpdateAsync(Guid id, UpdateTripDto dto, Guid userId);
        Task<Result> DeleteAsync(Guid id, Guid userId, bool isAdmin = false);
        Task<Result> DeleteAllByUser(Guid userId, bool isAdmin = false);
    }
}
