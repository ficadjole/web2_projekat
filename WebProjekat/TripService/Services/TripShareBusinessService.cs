

using Common.Enums;
using System.Fabric;
using TripService.Helpers;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.TripShare;
using TripService.Mappers;
using TripService.Models;
using WebProjekat.Common;

namespace TripService.Services
{
    public class TripShareBusinessService : ITripShareBusinessService
    {

        private readonly ITripShareRepository _tripShareRepository;
        private readonly ITripRepository _tripRepository;
        private readonly string _baseUrl;

        public TripShareBusinessService(ITripShareRepository tripShareRepository, ITripRepository tripRepository, StatelessServiceContext context)
        {
            _tripShareRepository = tripShareRepository;
            _tripRepository = tripRepository;

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            var baseAppSettings = config.Sections["BaseAppSettings"];

            _baseUrl = baseAppSettings.Parameters["BaseUrl"].Value;

        }

        public async Task<Result<TripShareDto>> CreateShareAsync(CreateTripShareDto dto, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(dto.TripId);

            if (trip is null)
                return Result<TripShareDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<TripShareDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            //token sluzi kao jedinstveni identifikator za deljenje putovanja, koristi se u URL-u za pristup deljenom putovanju
            var token = new TripShareHelpers().GenerateToken();

            var tripShare = TripShare.Create(dto.TripId.ToString(), token, dto.AccessType, DateTime.UtcNow.AddDays(dto.ExpiresInDays), DateTime.UtcNow);

            if (tripShare.IsFailure)
                return Result<TripShareDto>.Failure(tripShare.Error!.Message, ErrorType.Failure);

            await _tripShareRepository.AddAsync(tripShare.Value);

            var shareUrl = $"{_baseUrl}/{token}";

            var tripShareDto = new TripShareDto()
            {
                Id = tripShare.Value.Id,
                TripId = tripShare.Value.TripId,
                Token = tripShare.Value.Token,
                AccessType = tripShare.Value.AccessType,
                ExpiresAt = tripShare.Value.ExpiresAt,
                ShareUrl = shareUrl
            };

            return Result<TripShareDto>.Success(tripShareDto);

        }

        public async Task<Result<SharedTripDto>> GetSharedTripAsync(string token)
        {
            var tripShare = await _tripShareRepository.GetByTokenAsync(token);

            if (tripShare is null)
                return Result<SharedTripDto>.Failure("Share not found.", ErrorType.NotFound);

            if (tripShare.ExpiresAt < DateTime.UtcNow)
                return Result<SharedTripDto>.Failure("Share link has expired.", ErrorType.Validation);

            var trip = await _tripRepository.GetByIdWithDetailsAsync(tripShare.TripId);

            if (trip is null)
                return Result<SharedTripDto>.Failure("Trip not found.", ErrorType.NotFound);

            var tripDetailsDto = MapTripToDto.MapToDetailsDto(trip);

            var sharedTripDto = new SharedTripDto()
            {
                Trip = tripDetailsDto,
                AccessType = tripShare.AccessType
            };

            return Result<SharedTripDto>.Success(sharedTripDto);

        }

        public async Task<Result<IEnumerable<TripShareDto>>> GetSharesByTripIdAsync(Guid tripId, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<IEnumerable<TripShareDto>>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<TripShareDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var shares = await _tripShareRepository.GetByTripIdAsync(tripId);

            return Result<IEnumerable<TripShareDto>>.Success(shares.Select(s => new TripShareDto
            {
                Id = s.Id,
                TripId = s.TripId,
                Token = s.Token,
                AccessType = s.AccessType,
                ExpiresAt = s.ExpiresAt
            }));
        }

        public async Task<Result> RevokeShareAsync(Guid id, Guid userId)
        {
            var share = await _tripShareRepository.GetByIdAsync(id);
            if (share is null)
                return Result.Failure("Share not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(share.TripId);

            if (trip.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _tripShareRepository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
