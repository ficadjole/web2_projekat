using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTOs
{
    public class RegistrationRequest
    {
        [FromForm(Name = "email")]
        public string Email { get; set; } = string.Empty;

        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [FromForm(Name = "password")]
        public string Password { get; set; } = string.Empty;

        [FromForm(Name = "role")]
        public string Role { get; set; } = string.Empty;
    }
}
