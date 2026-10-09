using ModelLayer.Request;
using ModelLayer.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        Task<ResponseModel<string>> RegisterUserBL(RegistrationModel registrationModel);
        Task<ResponseModel<LoginResponseModel>> LoginUserBL(LoginModel loginModel);

    }
}
