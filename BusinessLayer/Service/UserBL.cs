using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using ModelLayer;


namespace BusinessLayer.Service
{
    public class UserBL:IUserBL
    {
        private IUserRL _userRL;
        private readonly IJwtService _jwtService;

        public UserBL(IUserRL userRL, IJwtService jwtService)
        {
            _userRL = userRL;
            _jwtService = jwtService;

        }
        public RegistrationModel RegisterUserBL(RegistrationModel registrationModel)
        {
            return _userRL.RegisterUserRL(registrationModel);
        }
        public LoginResponseModel LoginUserBL(LoginModel loginModel)
        {
            var user = _userRL.LoginUserRL(loginModel);
            var token = _jwtService.GenerateToken(
              user.UserId,
              user.Email
            );
            user.Token = token;
            return user;
        }


    }
}
