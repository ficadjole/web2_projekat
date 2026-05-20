using UserService.Models;

namespace UserService.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailRegisteredAsync(string email);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task<User> GetByEmailAsync(string email);

        Task<User> GetByIdAsync(Guid userId);

        Task DeleteAsync(Guid userId);

    }
}
