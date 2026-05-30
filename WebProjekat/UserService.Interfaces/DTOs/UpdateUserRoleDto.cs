using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Interfaces.DTOs
{
    [DataContract]
    public class UpdateUserRoleDto
    {
        [DataMember] 
        public string Role { get; set; } = string.Empty;
    }
}
