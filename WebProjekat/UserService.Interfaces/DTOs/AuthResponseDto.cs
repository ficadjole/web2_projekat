using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DTOs
{
    [DataContract]
    public class AuthResponseDto
    {
        [DataMember]
        public string Token { get; set; } = string.Empty;

        [DataMember]
        public UserDto User { get; set; } = new UserDto();
    }
}
