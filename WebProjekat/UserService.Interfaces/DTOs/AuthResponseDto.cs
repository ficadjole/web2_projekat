using System.Runtime.Serialization;

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
