using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TripService.Interfaces.DTOs.Destination
{
    [DataContract]
    public class UpdateDestinationDto
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public string Description { get; set; }= string.Empty;
        [DataMember]
        public string Notes { get; set; } = string.Empty;
        [DataMember]
        public string Location { get; set; } = string.Empty;
        [DataMember]
        public DateTime ArrivingDate { get; set; }
        [DataMember]
        public DateTime LeavingDate { get; set; }
    }
}
