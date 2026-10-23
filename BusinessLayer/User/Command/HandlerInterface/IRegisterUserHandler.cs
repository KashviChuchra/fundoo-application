using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.User.Command.HandlerInterface
{
    public interface IRegisterUserHandler
    {
        Task<ResponseModel<string>> HandleAsync(RegisterUserCommand command);
    }
}
