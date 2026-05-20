using Microsoft.ServiceFabric.Services.Remoting;
using UserService.DTOs;
using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IUserService : IService
    {
        Task<Result<AuthResponseDto>> LoginAsync(string username, string password);

        Task<Result<AuthResponseDto>> RegisterAsync(string name, string email, string password, string role);

    }
}
