
using System.Runtime.Serialization;

namespace TripService.Interfaces.DTOs.Trip
{
    [DataContract]
    public class TripDto
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
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public decimal PlannedBudget { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
    }
}
