using System;
using System.Collections.Generic;
using System.Text;
using ModelLayer;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        RegistrationModel RegisterUserRL(RegistrationModel registrationModel);
        LoginResponseModel LoginUserRL(LoginModel loginModel);
        Task<bool> ForgotPassword(ForgotPasswordModel model);

    }
}
