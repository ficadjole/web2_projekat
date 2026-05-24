using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IValidator<AuthRequest> validatorAuth, IValidator<RegistrationRequest> validatorRegistration, UserServiceProxy userServiceProxy) : ControllerBase
    {


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {

            var validationResult = validatorAuth.Validate(request);

            if (!validationResult.IsValid)
            {

                return BadRequest(validationResult.Errors);

            }

            var loginResult = userServiceProxy.GetUserProxy().LoginAsync(request.Email, request.Password).GetAwaiter().GetResult();

            if (!loginResult.IsSuccess)
            {
                return BadRequest(new { Message = "Login failed", Errors = loginResult.Error!.Message });
            }

            return Ok(new { loginResult.Value });

        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var validationResult = validatorRegistration.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            var registrationResult = userServiceProxy.GetUserProxy().RegisterAsync(request.Name, request.Email, request.Password, request.Role).GetAwaiter().GetResult();

            if (!registrationResult.IsSuccess)
            {
                return BadRequest(new { Message = "Registration failed", Errors = registrationResult.Error!.Message });
            }

            return Ok(new { registrationResult.Value });
        }
    }
}
