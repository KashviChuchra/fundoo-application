using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Middleware.Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail,string subject,string body)
        {
            var host = _configuration["SmtpSettings:Host"];
            var portValue = _configuration["SmtpSettings:Port"];
            var username = _configuration["SmtpSettings:Username"];
            var password = _configuration["SmtpSettings:Password"];

            if (string.IsNullOrWhiteSpace(host) ||
                !int.TryParse(portValue, out var port) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "SMTP configuration is missing or invalid.");
            }

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(username),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            using var smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
