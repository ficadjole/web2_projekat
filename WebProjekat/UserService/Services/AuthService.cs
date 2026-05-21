using Common.Enums;
using Common.Interfaces;
using Microsoft.Extensions.Logging;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;
using WebProjekat.Common;

namespace UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<AuthResponseDto>> AuthenticateAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("Authentication failed for email: {Email}", email);
                    return Result<AuthResponseDto>.Failure("Invalid email or password.",ErrorType.Authentication);
                }

                var isPasswordValid = _passwordHasher.VerifyPassword(password, user.Password);

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Authentication failed for email: {Email}", email);
                    return Result<AuthResponseDto>.Failure("Invalid email or password.", ErrorType.Authentication);
                }

                var token = _jwtService.GenerateToken(user.Id,user.Email,user.Role.ToString());

                _logger.LogInformation("User authenticated successfully: {Email}", email);
                
                var userDto = new UserDto() { Id = user.Id.ToString(),Name = user.Name, Email = user.Email, Role = user.Role.ToString() };

                var authResponseDto = new AuthResponseDto() { Token = token.Value, User = userDto };

                return Result<AuthResponseDto>.Success(authResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for email: {Email}", email);
                return Result<AuthResponseDto>.Failure("An error occurred during authentication. Please try again later.",ErrorType.Unexpected);
            }
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(string name, string email, string password, string role)
        {

            if(!Enum.TryParse<UserRoles>(role, true, out var parsedRole) && parsedRole != UserRoles.User)
            {
                return Result<AuthResponseDto>.Failure("Invalid user role specified.", ErrorType.Validation);
            }

            var user = new User(name, email, _passwordHasher.HashPassword(password), parsedRole);

            try
            {
                await _userRepository.AddAsync(user);

                _logger.LogInformation("User registered successfully: {Email}", email);

                var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

                if (token.IsFailure)
                {
                    return Result<AuthResponseDto>.Failure("User registered but failed to generate authentication token.", ErrorType.Unexpected);
                }

                var userDto = new UserDto() { Id = user.Id.ToString(), Name = user.Name, Email = user.Email, Role = user.Role.ToString() };

                var authResponseDto = new AuthResponseDto() { Token = token.Value, User = userDto };

                return Result<AuthResponseDto>.Success(authResponseDto);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", email);
                return Result<AuthResponseDto>.Failure("An error occurred during registration. Please try again later.", ErrorType.Unexpected);
            }


        }
    }
}
