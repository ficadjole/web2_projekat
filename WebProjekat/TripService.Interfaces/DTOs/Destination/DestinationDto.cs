
using System.Runtime.Serialization;
using TripService.Interfaces.DTOs.Activity;


namespace TripService.Interfaces.DTOs.Destination
{
    [DataContract]
    public class DestinationDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public string Description { get; set; } = string.Empty;
        [DataMember]
        public string Notes { get; set; } = string.Empty;
        [DataMember]
        public string Location { get; set; } = string.Empty;
        [DataMember]
        public DateTime ArrivingDate { get; set; }
        [DataMember]
        public DateTime LeavingDate { get; set; }
        [DataMember]
        public Guid TripId { get; set; }
        [DataMember]
        public IEnumerable<ActivityDto> Activities { get; set; }
    }
}
