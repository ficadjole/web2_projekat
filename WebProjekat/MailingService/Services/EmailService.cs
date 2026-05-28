using MailingService.Interfaces;
using MailingService.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MailingService.Services
{
    public class EmailService(SmtpOptions smtpOptions, ILogger<EmailService> logger) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(smtpOptions.SenderName, smtpOptions.SenderEmail));
                message.To.Add(new MailboxAddress(string.Empty, toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();

                await client.ConnectAsync(smtpOptions.Host, smtpOptions.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpOptions.SenderEmail, smtpOptions.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                throw;
            }
        }
    }
}
