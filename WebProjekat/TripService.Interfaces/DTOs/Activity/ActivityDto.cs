
using System.Runtime.Serialization;
using TripService.Enums;

namespace TripService.Interfaces.DTOs.Activity
{
    [DataContract]
    public class ActivityDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public string Location { get; set; } = string.Empty;
        [DataMember]
        public string Description { get; set; } = string.Empty;
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public decimal EstimatedCost { get; set; }
        [DataMember]
        public ActivityStatus Status { get; set; }
        [DataMember]
        public Guid DestinationId { get; set; }
    }
}
