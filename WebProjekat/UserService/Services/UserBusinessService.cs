using Common.Enums;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using TripService.Interfaces;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Interfaces.DTOs;
using UserService.Mappers;
using WebProjekat.Common;

namespace UserService.Services
{
    public class UserBusinessService(IUserRepository userRepository) : IUserBusinessService
    {
        public async Task<Result> DeleteUser(Guid userId)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(userId);

                if (user == null)
                {

                    return Result<string>.Failure($"User with ID {userId} does not exist", ErrorType.NotFound);

                }

                var proxyTrip = ServiceProxy.Create<ITripService>(new Uri("fabric:/WebProjekat/TripService"));

                var deleteTripsResult = await proxyTrip.DeleteAllByUser(userId, true);

                if (deleteTripsResult.IsFailure)
                    return Result<string>.Failure("Failed to delete user's trips. User deletion aborted.", ErrorType.Unexpected);


                await userRepository.DeleteAsync(userId);

                return Result.Success();

            }
            catch (Exception ex)
            {

                return Result<string>.Failure("An error occurred during authentication. Please try again later.", ErrorType.Unexpected);

            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {

                var users = await userRepository.GetAll();

                return Result<IEnumerable<UserDto>>.Success(users.Select(MapUserToDto.MapToDto));

            }
            catch (Exception ex)
            {

                return Result<IEnumerable<UserDto>>.Failure("An error occurred during authentication. Please try again later.", ErrorType.Unexpected);

            }
        }

        public async Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid requestingUserId)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
                return Result<UserDto>.Failure("User not found.", ErrorType.NotFound);

            if (user.Id != requestingUserId)
            {
                var requestingUser = await userRepository.GetByIdAsync(requestingUserId);
                if (requestingUser?.Role != UserRoles.Admin)
                    return Result<UserDto>.Failure("Unauthorized.", ErrorType.Unauthorized);
            }

            var emailExists = await userRepository.GetByEmailAsync(dto.Email);
            if (emailExists is not null && emailExists.Id != id)
                return Result<UserDto>.Failure("Email already in use.", ErrorType.Validation);

            user.Name = dto.Name;
            user.Email = dto.Email;

            await userRepository.UpdateAsync(user);

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }

        public async Task<Result<UserDto>> UpdateRoleAsync(Guid id, UpdateUserRoleDto dto)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
                return Result<UserDto>.Failure("User not found.", ErrorType.NotFound);

            if (!Enum.TryParse<UserRoles>(dto.Role, out var newRole))
                return Result<UserDto>.Failure("Invalid role.", ErrorType.Validation);

            user.Role = newRole;

            await userRepository.UpdateAsync(user);

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user is null)
                return Result<UserDto>.Failure("User not found.", ErrorType.NotFound);

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }
    }
}
