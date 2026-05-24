using UserService.DTOs;
using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IUserBusinessService
    {
        Task<Result<IEnumerable<UserDto>>> GetAllUsers();
        Task<Result> DeleteUser(Guid userId);
    }
}
