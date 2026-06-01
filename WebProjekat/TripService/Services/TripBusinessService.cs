using Common.Enums;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.Trip;
using TripService.Mappers;
using TripService.Models;
using WebProjekat.Common;

namespace TripService.Services
{
    public class TripBusinessService : ITripBusinessService
    {
        private readonly ITripRepository _tripRepository;

        public TripBusinessService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<Result<TripDto>> CreateAsync(CreateTripDto dto, Guid userId)
        {
            var result = Trip.Create(dto.Name, dto.Description, dto.StartDate,
                                     dto.EndDate, dto.PlannedBudget, userId);
            if (result.IsFailure)
                return Result<TripDto>.Failure(result.Error!.Message, result.Error.Type);

            await _tripRepository.AddAsync(result.Value);
            return Result<TripDto>.Success(MapTripToDto.MapToDto(result.Value));
        }

        public async Task<Result<TripDto>> GetByIdAsync(Guid id, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip is null)
                return Result<TripDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<TripDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            return Result<TripDto>.Success(MapTripToDto.MapToDto(trip));
        }

        public async Task<Result<TripDetailsDto>> GetWithDetailsAsync(Guid id, Guid userId)
        {
            var trip = await _tripRepository.GetByIdWithDetailsAsync(id);
            if (trip is null)
                return Result<TripDetailsDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<TripDetailsDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var totalExpenses = trip.Expenses?.Sum(e => e.Amount) ?? 0;
            var activitiesTotal = trip.Destinations.SelectMany(d => d.Activities).Sum(a => a.EstimatedCost);

            var dto = new TripDetailsDto
            {
                Id = trip.Id,
                Name = trip.Name,
                Description = trip.Description,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                PlannedBudget = trip.PlannedBudget,
                UserId = trip.UserId,
                TotalExpenses = totalExpenses+activitiesTotal,
                RemainingBudget = trip.PlannedBudget - totalExpenses - activitiesTotal,
                Destinations = trip.Destinations?.Select(MapDestinationToDto.MapToDto) ?? [],
                Expenses = trip.Expenses?.Select(MapExpenseToDto.MapToDto) ?? []
            };

            return Result<TripDetailsDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<TripDto>>> GetAllByUserAsync(Guid userId)
        {
            var trips = await _tripRepository.GetAllByUserIdAsync(userId);
            return Result<IEnumerable<TripDto>>.Success(trips.Select(MapTripToDto.MapToDto));
        }

        public async Task<Result<TripDto>> UpdateAsync(Guid id, UpdateTripDto dto, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip is null)
                return Result<TripDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<TripDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.StartDate > dto.EndDate)
                return Result<TripDto>.Failure("StartDate cannot be after EndDate.", ErrorType.Validation);

            if (dto.PlannedBudget < 0)
                return Result<TripDto>.Failure("PlannedBudget cannot be negative.", ErrorType.Validation);

            trip.Name = dto.Name;
            trip.Description = dto.Description;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;
            trip.PlannedBudget = dto.PlannedBudget;

            await _tripRepository.UpdateAsync(trip);
            return Result<TripDto>.Success(MapTripToDto.MapToDto(trip));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip is null)
                return Result.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _tripRepository.DeleteAsync(id);
            return Result.Success();
        }

    }
}
