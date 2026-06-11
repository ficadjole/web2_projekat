using System.Runtime.Serialization;
using TripService.Interfaces.DTOs.TripShare;

namespace MailingService.Interface.Events
{
    [DataContract]
    public class EmailEvent
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;
        [DataMember]
        public TripShareDto TripShareDto { get; set; }
    }
}
