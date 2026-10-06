using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        RegistrationModel RegisterUserBL(RegistrationModel registrationModel);
        LoginResponseModel LoginUserBL(LoginModel loginModel);

    }
}
