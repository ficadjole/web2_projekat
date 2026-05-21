
using TripService.Interfaces.DTOs.Activity;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IActivityBusinessService
    {
        Task<Result<ActivityDto>> CreateAsync(CreateActivityDto dto, Guid userId);
        Task<Result<ActivityDto>> GetByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<ActivityDto>>> GetByDestinationIdAsync(Guid destinationId, Guid userId);
        Task<Result<IEnumerable<ActivityDto>>> GetByDateAsync(Guid tripId, DateTime date, Guid userId);
        Task<Result<ActivityDto>> UpdateAsync(Guid id, UpdateActivityDto dto, Guid userId);
        Task<Result> DeleteAsync(Guid id, Guid userId);
    }
}
