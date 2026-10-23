using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Request.UserRequest;

namespace BusinessLayer.User.Command
{
    public class RegisterUserCommand
    {
        public RegistrationModel User { get; }
        public RegisterUserCommand(RegistrationModel user)
        {
            User = user;
        }
    }
}
