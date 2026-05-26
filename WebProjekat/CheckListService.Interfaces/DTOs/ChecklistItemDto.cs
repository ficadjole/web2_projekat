using System.Runtime.Serialization;

namespace CheckListService.Interfaces.DTOs
{
    [DataContract]
    public class ChecklistItemDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public bool IsChecked { get; set; }

        [DataMember]
        public Guid ChecklistId { get; set; }
    }
}
