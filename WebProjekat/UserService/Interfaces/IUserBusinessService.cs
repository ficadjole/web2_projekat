using UserService.DTOs;
using UserService.Interfaces.DTOs;
using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IUserBusinessService
    {
        Task<Result<UserDto>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<UserDto>>> GetAllUsers();
        Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid requestingUserId);
        Task<Result<UserDto>> UpdateRoleAsync(Guid id, UpdateUserRoleDto dto);
        Task<Result> DeleteUser(Guid userId);
    }
}
