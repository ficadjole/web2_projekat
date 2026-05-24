using UserService.DTOs;
using UserService.Models;

namespace UserService.Mappers
{
    public static class MapUserToDto
    {
        public static UserDto MapToDto(User user) => new()
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };

    }
}
