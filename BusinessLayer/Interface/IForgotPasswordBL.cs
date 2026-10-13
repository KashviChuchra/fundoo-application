using ModelLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Request.UserRequest;

namespace BusinessLayer.Interface
{
    public interface IForgotPasswordBL
    {
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest model);
    }
}
