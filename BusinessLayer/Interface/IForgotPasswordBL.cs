using ModelLayer.Response;
using ModelLayer.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IForgotPasswordBL
    {
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest model);
    }
}
