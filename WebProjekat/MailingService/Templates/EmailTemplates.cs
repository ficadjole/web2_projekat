using Common.Enums;
using TripService.Interfaces.DTOs.TripShare;

namespace MailingService.Templates
{
    public static class EmailTemplates
    {
        public static string TripShareTemplate(TripShareDto shareDto)
        {
            var accessTypeText = shareDto.AccessType == ShareAccessType.View ? "View Only" : "View & Edit";
            var accessTypeColor = shareDto.AccessType == ShareAccessType.View ? "#3498db" : "#2ecc71";

            string qrImageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(shareDto.ShareUrl)}";

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 40px auto;
                        background-color: #ffffff;
                        border-radius: 10px;
                        padding: 40px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                    }}
                    .header {{
                        text-align: center;
                        margin-bottom: 30px;
                    }}
                    .header h1 {{
                        color: #2c3e50;
                        font-size: 24px;
                        margin: 0;
                    }}
                    .access-badge {{
                        display: inline-block;
                        background-color: {accessTypeColor};
                        color: white;
                        padding: 6px 16px;
                        border-radius: 20px;
                        font-size: 14px;
                        font-weight: bold;
                        margin-top: 10px;
                    }}
                    .qr-container {{
                        text-align: center;
                        margin: 30px 0;
                        padding: 20px;
                        background-color: #f9f9f9;
                        border-radius: 8px;
                        border: 1px solid #e0e0e0;
                    }}
                    .qr-container img {{
                        width: 200px;
                        height: 200px;
                    }}
                    .qr-container p {{
                        color: #7f8c8d;
                        font-size: 13px;
                        margin-top: 10px;
                    }}
                    .expiry {{
                        text-align: center;
                        color: #e74c3c;
                        font-size: 13px;
                        margin-top: 20px;
                    }}
                    .footer {{
                        text-align: center;
                        margin-top: 30px;
                        color: #bdc3c7;
                        font-size: 12px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Trip Plan Shared With You</h1>
                        <span class='access-badge'>{accessTypeText}</span>
                    </div>

                    <div class='qr-container'>
                        <img src='{qrImageUrl}' alt='QR Code' />
                        <p>Scan the QR code to access the trip plan</p>
                    </div>

                    <div class='expiry'>
                        Link expires on: {shareDto.ExpiresAt:dd MMM yyyy HH:mm} UTC
                    </div>

                    <div class='footer'>
                        <p>This email was sent from ProjekatWeb. Please do not reply.</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
