using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Request.UserRequest;

namespace BusinessLayer.User.Command
{
    public class ResetPasswordCommand
    {
        public ResetPasswordRequest Request { get; }
        public ResetPasswordCommand(ResetPasswordRequest request)
        {
            Request = request;
        }
    }
}
