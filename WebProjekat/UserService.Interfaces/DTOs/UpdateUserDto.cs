using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Interfaces.DTOs
{
    [DataContract]
    public class UpdateUserDto
    {
        [DataMember] 
        public string Name { get; set; } = string.Empty;

        [DataMember] 
        public string Email { get; set; } = string.Empty;
    }
}
