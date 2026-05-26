using System.Runtime.Serialization;

namespace CheckListService.Interfaces.DTOs
{
    [DataContract]
    public class ChecklistDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid TripId { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public List<ChecklistItemDto> Items { get; set; }
    }
}
