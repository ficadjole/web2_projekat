using UserService.DTOs;
using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> AuthenticateAsync(string email, string password);

        Task<Result<AuthResponseDto>> RegisterAsync(string name, string email, string password, string role);
    }
}
