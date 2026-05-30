using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Interfaces.DTOs;
using WebApi.DTOs.User;
using WebApi.Services;

namespace WebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserServiceProxy userServiceProxy, IValidator<UpdateUserRequest> updateUserValidator, IValidator<UpdateUserRoleRequest> updateUserRoleValidator) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {

            var usersResult = userServiceProxy.GetUserProxy().GetUsersAsync().GetAwaiter().GetResult();

            if (!usersResult.IsSuccess)
            {
                return BadRequest(new { Message = "Get all users failed", Errors = usersResult.Error!.Message });
            }

            return Ok(usersResult.Value);

        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            if (id == null)
            {
                return BadRequest(new { Message = "UserId cannot be null" });
            }


            var deleteResult = userServiceProxy.GetUserProxy().DeleteUserAsync(id).GetAwaiter().GetResult();

            if (deleteResult.IsFailure)
            {

                return BadRequest(new { Message = "Deleting user was not succesfull" });

            }

            return Ok("User deleted successfully.");

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            var validatorResult = await updateUserValidator.ValidateAsync(request);
            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateUserDto
            {
                Name = request.Name,
                Email = request.Email
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await userServiceProxy.GetUserProxy().UpdateUserAsync(id, dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPatch("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateUserRoleRequest request)
        {
            var validatorResult = await updateUserRoleValidator.ValidateAsync(request);
            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateUserRoleDto { Role = request.Role };

            var result = await userServiceProxy.GetUserProxy().UpdateRoleAsync(id, dto);
            if (result.IsFailure)
                return BadRequest(result.Error!.Message);
            return Ok(result.Value);
        }
    }
}
