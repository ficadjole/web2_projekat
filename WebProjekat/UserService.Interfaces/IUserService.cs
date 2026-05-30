using Microsoft.ServiceFabric.Services.Remoting;
using UserService.DTOs;
using UserService.Interfaces.DTOs;
using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IUserService : IService
    {
        Task<Result<AuthResponseDto>> LoginAsync(string username, string password);

        Task<Result<AuthResponseDto>> RegisterAsync(string name, string email, string password, string role);

        Task<Result<IEnumerable<UserDto>>> GetUsersAsync();

        Task<Result> DeleteUserAsync(string id);

        Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid requestingUserId);
        Task<Result<UserDto>> UpdateRoleAsync(Guid id, UpdateUserRoleDto dto);
        Task<Result<UserDto>> GetByIdAsync(Guid id);

    }
}
