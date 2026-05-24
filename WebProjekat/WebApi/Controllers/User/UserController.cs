using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserServiceProxy userServiceProxy) : ControllerBase
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

        [HttpDelete]
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
    }
}
