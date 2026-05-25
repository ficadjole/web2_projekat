using Common.Enums;
using System.Runtime.Serialization;
using TripService.Interfaces.DTOs.Trip;

namespace TripService.Interfaces.DTOs.TripShare
{
    [DataContract]
    public class SharedTripDto
    {
        [DataMember]
        public TripDetailsDto Trip { get; set; }

        [DataMember]
        public ShareAccessType AccessType { get; set; }
    }
}
