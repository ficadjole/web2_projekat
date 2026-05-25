
using Common.Enums;

namespace TripService.Interfaces.DTOs.TripShare
{
    public class CreateTripShareRequest
    {
        public Guid TripId { get; set; }

        public ShareAccessType AccessType { get; set; }

        public int ExpiresInDays { get; set; }

    }
}
