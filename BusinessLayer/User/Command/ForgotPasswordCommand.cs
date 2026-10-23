using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Request.UserRequest;

namespace BusinessLayer.User.Command
{
    public class ForgotPasswordCommand
    {
        public ForgotPasswordRequest Request { get; }
        public ForgotPasswordCommand(ForgotPasswordRequest request)
        {
            Request = request;
        }
    }
}
