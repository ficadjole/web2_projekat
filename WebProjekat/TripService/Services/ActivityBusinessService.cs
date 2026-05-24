using Common.Enums;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.Activity;
using TripService.Mappers;
using TripService.Models;
using WebProjekat.Common;

namespace TripService.Services
{
    public class ActivityBusinessService : IActivityBusinessService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly ITripRepository _tripRepository;

        public ActivityBusinessService(IActivityRepository activityRepository,
                                       IDestinationRepository destinationRepository,
                                       ITripRepository tripRepository)
        {
            _activityRepository = activityRepository;
            _destinationRepository = destinationRepository;
            _tripRepository = tripRepository;
        }

        public async Task<Result<ActivityDto>> CreateAsync(CreateActivityDto dto, Guid userId)
        {
            var destination = await _destinationRepository.GetByIdAsync(dto.DestinationId);

            if (destination is null)
                return Result<ActivityDto>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);

            if (trip is null)
                return Result<ActivityDto>.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<ActivityDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.EstimatedCost < 0)
                return Result<ActivityDto>.Failure("EstimatedCost cannot be negative.", ErrorType.Validation);

            var activity = Activity.Create(dto.Name, dto.Location, dto.Description,
                                        dto.Date, dto.EstimatedCost, dto.Status, dto.DestinationId.ToString());

            if (activity.IsFailure)
                return Result<ActivityDto>.Failure("Cannot create activity", ErrorType.Unexpected);

            await _activityRepository.AddAsync(activity.Value);

            return Result<ActivityDto>.Success(MapActivityToDto.MapToDto(activity.Value));
        }

        public async Task<Result<ActivityDto>> GetByIdAsync(Guid id, Guid userId)
        {
            var activity = await _activityRepository.GetByIdAsync(id);

            if (activity is null)
                return Result<ActivityDto>.Failure("Activity not found.", ErrorType.NotFound);

            var destination = await _destinationRepository.GetByIdAsync(activity.DestinationId);

            if (destination is null)
                return Result<ActivityDto>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);

            if (trip is null)
                return Result<ActivityDto>.Failure("Trip not found", ErrorType.NotFound);


            if (trip.UserId != userId)
                return Result<ActivityDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            return Result<ActivityDto>.Success(MapActivityToDto.MapToDto(activity));
        }

        public async Task<Result<IEnumerable<ActivityDto>>> GetByDestinationIdAsync(Guid destinationId, Guid userId)
        {
            var destination = await _destinationRepository.GetByIdAsync(destinationId);
            if (destination is null)
                return Result<IEnumerable<ActivityDto>>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);

            if (trip is null)
                return Result<IEnumerable<ActivityDto>>.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<ActivityDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var activities = await _activityRepository.GetByDestinationIdAsync(destinationId);
            return Result<IEnumerable<ActivityDto>>.Success(activities.Select(MapActivityToDto.MapToDto));
        }

        public async Task<Result<IEnumerable<ActivityDto>>> GetByDateAsync(Guid tripId, DateTime date, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<IEnumerable<ActivityDto>>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<ActivityDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var activities = await _activityRepository.GetByDateAsync(tripId, date);
            return Result<IEnumerable<ActivityDto>>.Success(activities.Select(MapActivityToDto.MapToDto));
        }

        public async Task<Result<ActivityDto>> UpdateAsync(Guid id, UpdateActivityDto dto, Guid userId)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity is null)
                return Result<ActivityDto>.Failure("Activity not found.", ErrorType.NotFound);

            var destination = await _destinationRepository.GetByIdAsync(activity.DestinationId);


            if (destination is null)
                return Result<ActivityDto>.Failure("Destination not found.", ErrorType.NotFound);


            var trip = await _tripRepository.GetByIdAsync(destination.TripId);


            if (trip is null)
                return Result<ActivityDto>.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<ActivityDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.EstimatedCost < 0)
                return Result<ActivityDto>.Failure("EstimatedCost cannot be negative.", ErrorType.Validation);

            activity.Name = dto.Name;
            activity.Location = dto.Location;
            activity.Description = dto.Description;
            activity.Date = dto.Date;
            activity.EstimatedCost = dto.EstimatedCost;
            activity.Status = dto.Status;

            await _activityRepository.UpdateAsync(activity);
            return Result<ActivityDto>.Success(MapActivityToDto.MapToDto(activity));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid userId)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity is null)
                return Result.Failure("Activity not found.", ErrorType.NotFound);

            var destination = await _destinationRepository.GetByIdAsync(activity.DestinationId);


            if (destination is null)
                return Result<ActivityDto>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);

            if (trip is null)
                return Result<ActivityDto>.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _activityRepository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
