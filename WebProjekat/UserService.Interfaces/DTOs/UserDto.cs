using System.Runtime.Serialization;

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
        public string Role { get; set; } = string.Empty;
    }
}
