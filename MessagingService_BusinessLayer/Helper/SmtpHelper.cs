using MessagingService_ModelLayer;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MessagingService_BusinessLayer.Helper
{
    public class SmtpHelper
    {
        private readonly IConfiguration _configuration;

        public SmtpHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailModel emailModel)
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
                Subject = emailModel.Subject,
                Body = emailModel.Body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(emailModel.ToEmail);

            using var smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
