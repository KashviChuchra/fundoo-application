using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using ModelLayer.Request;
using ModelLayer.Response;


namespace BusinessLayer.Service
{
    public class UserBL:IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;

        public UserBL(IUserRL userRL, IJwtService jwtService)
        {
            _userRL = userRL;
            _jwtService = jwtService;

        }
        public async Task<ResponseModel<string>> RegisterUserBL(RegistrationModel registrationModel)
        {
            return await _userRL.RegisterUserRL(registrationModel);
        }
        public async Task<ResponseModel<LoginResponseModel>> LoginUserBL(LoginModel loginModel)
        {
            var response = await _userRL.LoginUserRL(loginModel);
            if (!response.Success || response.Data==null)
            {
                return response;
            }

            var user = response.Data;

            var token = _jwtService.GenerateToken(user.UserId, user.Email);

            user.Token = token;

            return new ResponseModel<LoginResponseModel>
            {
                Success = true,
                Message = "Login successful",
                Data = user
            };
        }


    }
}
