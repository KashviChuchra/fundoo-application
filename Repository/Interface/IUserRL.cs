using ModelLayer.Request;
using ModelLayer.Response;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        Task<ResponseModel<string>> RegisterUserRL(RegistrationModel registrationModel);
        Task<ResponseModel<LoginResponseModel>> LoginUserRL(LoginModel loginModel);
    }
}
