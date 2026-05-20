using Microsoft.AspNetCore.Mvc;
using WebProjekat.Common;

namespace WebApi.DTOs
{
    public class AuthRequest
    {
        [FromForm(Name = "email")]
        public string Email { get; set; } = string.Empty;

        [FromForm(Name = "password")]

        public string Password { get; set; } = string.Empty;

    }
}
