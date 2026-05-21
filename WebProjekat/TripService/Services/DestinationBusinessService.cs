using Common.Enums;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.Destination;
using TripService.Mappers;
using TripService.Models;
using WebProjekat.Common;

namespace TripService.Services
{
    public class DestinationBusinessService : IDestinationBusinessService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly ITripRepository _tripRepository;

        public DestinationBusinessService(IDestinationRepository destinationRepository,
                                          ITripRepository tripRepository)
        {
            _destinationRepository = destinationRepository;
            _tripRepository = tripRepository;
        }

        public async Task<Result<DestinationDto>> CreateAsync(CreateDestinationDto dto, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(dto.TripId);
            if (trip is null)
                return Result<DestinationDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<DestinationDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.ArrivingDate > dto.LeavingDate)
                return Result<DestinationDto>.Failure("ArrivingDate cannot be after LeavingDate.", ErrorType.Validation);

            var destination = Destination.Create(dto.Name, dto.Description,
                                              dto.Notes, dto.Location, dto.ArrivingDate,
                                              dto.LeavingDate, dto.TripId.ToString());

            if (destination.IsFailure)
                return Result<DestinationDto>.Failure("Cannot create destination", ErrorType.Unexpected);

            await _destinationRepository.AddAsync(destination.Value);
            return Result<DestinationDto>.Success(MapDestinationToDto.MapToDto(destination.Value));
        }

        public async Task<Result<DestinationDto>> GetByIdAsync(Guid id, Guid userId)
        {
            var destination = await _destinationRepository.GetByIdWithActivitiesAsync(id);
            if (destination is null)
                return Result<DestinationDto>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);
            if (trip.UserId != userId)
                return Result<DestinationDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            return Result<DestinationDto>.Success(MapDestinationToDto.MapToDto(destination));
        }

        public async Task<Result<IEnumerable<DestinationDto>>> GetByTripIdAsync(Guid tripId, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<IEnumerable<DestinationDto>>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<DestinationDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var destinations = await _destinationRepository.GetByTripIdAsync(tripId);
            return Result<IEnumerable<DestinationDto>>.Success(destinations.Select(MapDestinationToDto.MapToDto));
        }

        public async Task<Result<DestinationDto>> UpdateAsync(Guid id, UpdateDestinationDto dto, Guid userId)
        {
            var destination = await _destinationRepository.GetByIdAsync(id);
            if (destination is null)
                return Result<DestinationDto>.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);
            if (trip.UserId != userId)
                return Result<DestinationDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.ArrivingDate > dto.LeavingDate)
                return Result<DestinationDto>.Failure("ArrivingDate cannot be after LeavingDate.", ErrorType.Validation);

            destination.Name = dto.Name;
            destination.Description = dto.Description;
            destination.Notes = dto.Notes;
            destination.Location = dto.Location;
            destination.ArrivingDate = dto.ArrivingDate;
            destination.LeavingDate = dto.LeavingDate;

            await _destinationRepository.UpdateAsync(destination);
            return Result<DestinationDto>.Success(MapDestinationToDto.MapToDto(destination));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid userId)
        {
            var destination = await _destinationRepository.GetByIdAsync(id);
            if (destination is null)
                return Result.Failure("Destination not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(destination.TripId);
            if (trip.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _destinationRepository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
