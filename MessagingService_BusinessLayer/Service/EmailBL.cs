using MessagingService_BusinessLayer.Helper;
using MessagingService_BusinessLayer.Interface;
using MessagingService_ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService_BusinessLayer.Service
{
    public class EmailBL :IEmailBL
    {
        private readonly SmtpHelper _smtpHelper;
        public EmailBL(SmtpHelper smtpHelper)
        {
            _smtpHelper = smtpHelper;
        }
        public async Task SendEmailAsync(EmailModel emailModel)
        {
            await _smtpHelper.SendEmailAsync(emailModel);
        }
    }
}
