using Common.Enums;
using WebProjekat.Common;

namespace TripService.Models
{
    public class TripShare
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }

        public string Token { get; set; } = string.Empty;

        public ShareAccessType AccessType { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Trip Trip { get; set; }

        protected TripShare() { }

        public Result<TripShare> Create(string tripId, string token, ShareAccessType accessType, DateTime expiresAt, DateTime createdAt)
        {
            if (string.IsNullOrEmpty(token))
                return Result<TripShare>.Failure("Token cannot be null",ErrorType.Validation);

            if (!Guid.TryParse(tripId, out Guid guid))
                return Result<TripShare>.Failure("Id not in right format", ErrorType.Validation);

            if (!Enum.IsDefined(typeof(ShareAccessType), accessType))
                return Result<TripShare>.Failure("AccessType not valid", ErrorType.Validation);

            var newTripShare = new TripShare() { Id = Guid.NewGuid(), Token = token, AccessType = accessType, ExpiresAt = expiresAt, CreatedAt = createdAt, TripId = guid };

            return Result<TripShare>.Success(newTripShare);

        }

        public Result<TripShare> Load(string id, Guid tripId, string token, ShareAccessType accessType, DateTime expiresAt, DateTime createdAt)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return Result<TripShare>.Failure("Id not in right format", ErrorType.Validation);

            var newTripShare = new TripShare() { Id = guid, Token = token, AccessType = accessType, ExpiresAt = expiresAt, CreatedAt = createdAt, TripId = tripId };

            return Result<TripShare>.Success(newTripShare);
        }
    }
}
