using Microsoft.EntityFrameworkCore;
using UserService.DatabaseContext;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _dbContext;
        public UserRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                return _dbContext.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }

        public Task<User> GetByEmailAsync(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {

                return Task.FromResult<User>(null);

            }

            return Task.FromResult(user);
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            var user = _dbContext.Users.Find(id);

            if (user == null)
            {

                return Task.FromResult<User?>(null);
            }

            return Task.FromResult<User?>(user);
        }

        public Task<bool> IsEmailRegisteredAsync(string email)
        {
            var isRegistered = _dbContext.Users.Any(u => u.Email == email);

            if (isRegistered)
            {

                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task UpdateAsync(User user)
        {
            var existingUser = _dbContext.Users.Find(user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;
                return _dbContext.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> GetAll() => await _dbContext.Users.ToListAsync();


    }
}
