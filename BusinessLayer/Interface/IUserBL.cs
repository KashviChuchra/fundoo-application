using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        Task<ResponseModel<string>> RegisterUserBL(RegistrationModel registrationModel);
        Task<ResponseModel<LoginResponseModel>> LoginUserBL(LoginModel loginModel);
        Task<ResponseModel<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request);

    }
}
