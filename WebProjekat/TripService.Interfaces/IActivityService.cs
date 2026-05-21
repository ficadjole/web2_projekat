using Microsoft.ServiceFabric.Services.Remoting;
using TripService.Interfaces.DTOs.Activity;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IActivityService : IService
    {
        Task<Result<ActivityDto>> CreateActivityAsync(CreateActivityDto dto, Guid userId);
        Task<Result<ActivityDto>> GetActivityByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<ActivityDto>>> GetActivitiesByDestinationIdAsync(Guid destinationId, Guid userId);
        Task<Result<IEnumerable<ActivityDto>>> GetActivitiesByDateAsync(Guid tripId, DateTime date, Guid userId);
        Task<Result<ActivityDto>> UpdateActivityAsync(Guid id, UpdateActivityDto dto, Guid userId);
        Task<Result> DeleteActivityAsync(Guid id, Guid userId);
    }
}
