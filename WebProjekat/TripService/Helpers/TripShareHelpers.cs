using QRCoder;
using System.Security.Cryptography;

namespace TripService.Helpers
{
    public class TripShareHelpers
    {
        public string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).Replace("+", "-")
                                                                                                   .Replace("/", "_")
                                                                                                   .Replace("=", "");

    }
}
