using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using WebProjekat.Common;

namespace UserService.Models
{
    [Table("Users")]

    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRoles Role { get; set; } = UserRoles.User;

        public User(string name, string email, string password, UserRoles role, Guid id = new Guid())
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public static Result<User> Create(
                string name,
                string email,
                string password,
                UserRoles role = UserRoles.User)
        {

            if (string.IsNullOrWhiteSpace(name))
                return Result<User>.Failure("Name is required.", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(email))
                return Result<User>.Failure("Email is required.", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(password))
                return Result<User>.Failure("Password is required.", ErrorType.Validation);

            if (!Enum.IsDefined(typeof(UserRoles), role))
                return Result<User>.Failure("Invalid user role.", ErrorType.Validation);

            var user = new User(name, email, password, role, new Guid());

            return Result<User>.Success(user);
        }

        public static Result<User> Load(
            string id,
            string name,
            string email,
            string passwordHash,
            UserRoles role)
        {

            if (Guid.TryParse(id, out var guid) == false)
                return Result<User>.Failure("Invalid user ID.", ErrorType.Validation);


            var user = new User(name, email, passwordHash, role, guid);


            return Result<User>.Success(user);
        }

    }
}
