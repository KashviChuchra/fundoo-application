using Middleware.Services.Jwt;
using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.User.Query
{
    public class LoginUserQueryHandler: ILoginUserQueryHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtTokenService _jwtTokenService;


        public LoginUserQueryHandler(IUserRL userRL, IJwtTokenService jwtTokenService)
        {
            _userRL = userRL;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ResponseModel<LoginResponseModel>> HandleAsync(LoginUserQuery query)
        {
            var user = await _userRL.LoginUserRL(query.LoginModel);
            if (user == null)
            {
                return new ResponseModel<LoginResponseModel>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Data = null
                };
            }
            var response = new LoginResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = _jwtTokenService.GenerateToken(user.UserId, user.Email)
            };

            return new ResponseModel<LoginResponseModel>
            {
                Success = true,
                Message = "Login successful.",
                Data = response
            };
        }
    }
}