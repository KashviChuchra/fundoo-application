using MessagingService_ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService_BusinessLayer.Interface
{
    public interface IEmailBL
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}
