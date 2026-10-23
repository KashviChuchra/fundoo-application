using ModelLayer.Request.UserRequest;

namespace BusinessLayer.User.Query
{
    public class LoginUserQuery
    {
        public LoginModel LoginModel { get; }

        public LoginUserQuery(LoginModel loginModel)
        {
            LoginModel = loginModel;
        }
    }
}