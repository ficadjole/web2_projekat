using Common.Interfaces;
using UserService.Models;

namespace UserService.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsEmailRegisteredAsync(string email);

        Task<User> GetByEmailAsync(string email);

    }
}
