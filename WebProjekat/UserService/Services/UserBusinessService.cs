using Common.Enums;
using UserService.DTOs;
using UserService.Interfaces;
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
    }
}
