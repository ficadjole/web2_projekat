using System.Runtime.Serialization;

namespace UserService.Interfaces.DTOs
{
    [DataContract]
    public class UpdateUserRoleDto
    {
        [DataMember]
        public string Role { get; set; } = string.Empty;
    }
}
