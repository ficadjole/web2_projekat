using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using UserService.Interfaces;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IValidator<AuthRequest> validatorAuth, IValidator<RegistrationRequest> validatorRegistration) : ControllerBase
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

            var proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/WebProjekat/UserService"));

            try
            {

                var loginResult = proxy.LoginAsync(request.Email, request.Password).GetAwaiter().GetResult();

                if (!loginResult.IsSuccess)
                {
                    return BadRequest(new { Message = "Login failed", Errors = loginResult.Error!.Message });
                }

                return Ok(new {loginResult.Value });

            }
            catch (Exception ex) { 
            
                return BadRequest(ex.Message);

            }
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

            var proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/WebProjekat/UserService"));

            try
            {

                var registrationResult = proxy.RegisterAsync(request.Name, request.Email, request.Password, request.Role).GetAwaiter().GetResult();

                if (!registrationResult.IsSuccess)
                {
                    return BadRequest(new { Message = "Registration failed", Errors = registrationResult.Error!.Message });
                }

                return Ok(new { registrationResult.Value });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
