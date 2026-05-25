using QRCoder;
using System.Security.Cryptography;

namespace TripService.Helpers
{
    public class TripShareHelpers
    {
        public string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).Replace("+", "-")
                                                                                                   .Replace("/", "_")
                                                                                                   .Replace("=", "");

        public string GenerateQrCode(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);

            return Convert.ToBase64String(qrCodeBytes);

        }
    }
}
