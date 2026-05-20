using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DTOs
{
    [DataContract]
    public class UserDto
    {
        [DataMember]
        public string Id { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Role {  get; set; } = string.Empty;
    }
}
