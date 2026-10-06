using MessagingService_ModelLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var port = int.Parse(_configuration["SmtpSettings:Port"]!);
            var username = _configuration["SmtpSettings:Username"];
            var password = _configuration["SmtpSettings:Password"];

            // SMTP implementation will come here
            var mailMessage = new MailMessage
            {
                From = new MailAddress(username!),
                Subject = emailModel.Subject,
                Body = emailModel.Body
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
