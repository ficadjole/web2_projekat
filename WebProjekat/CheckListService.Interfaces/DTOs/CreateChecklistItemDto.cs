using System.Runtime.Serialization;

namespace CheckListService.Interfaces.DTOs
{
    [DataContract]
    public class CreateChecklistItemDto
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public Guid TripId { get; set; }
    }
}
