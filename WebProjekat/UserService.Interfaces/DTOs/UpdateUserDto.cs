using System.Runtime.Serialization;

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
