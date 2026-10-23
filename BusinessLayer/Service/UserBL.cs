
using BCrypt.Net;
using BusinessLayer.Interface;
using BusinessLayer.User.Command;
using BusinessLayer.User.Query;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Middleware.Services.Email;
using Middleware.Services.Hashing;
using Middleware.Services.Jwt;
using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;
using System.Security.Cryptography;
using BusinessLayer.User.Command.HandlerInterface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserBL> _logger;
        private readonly CustomHashing _customHasher;
        private readonly IRegisterUserHandler _registerUserHandler;
        private readonly IForgotPasswordHandler _forgotPasswordHandler;
        private readonly IResetPasswordHandler _resetPasswordHandler;
        private readonly ILoginUserQueryHandler _loginUserQueryHandler;

        public UserBL(
            IUserRL userRL,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILogger<UserBL> logger,
            CustomHashing customHasher,
            IRegisterUserHandler registerUserHandler,
            IForgotPasswordHandler forgotPasswordHandler,
            IResetPasswordHandler resetPasswordHandler,
            ILoginUserQueryHandler loginUserQueryHandler)
        {
            _userRL = userRL;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
            _customHasher = customHasher;
            _registerUserHandler = registerUserHandler;
            _forgotPasswordHandler = forgotPasswordHandler;
            _resetPasswordHandler = resetPasswordHandler;
            _loginUserQueryHandler = loginUserQueryHandler;
        }

        public async Task<ResponseModel<string>> RegisterUserBL(RegistrationModel registrationModel)
        {
            var command = new RegisterUserCommand(registrationModel);
            return await _registerUserHandler.HandleAsync(command);
        }

        public async Task<ResponseModel<LoginResponseModel>> LoginUserBL(LoginModel loginModel)
        {
            var query = new LoginUserQuery(loginModel);
            return await _loginUserQueryHandler.HandleAsync(query);
        }

        public async Task<ResponseModel<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var command = new ForgotPasswordCommand(request);
            return await _forgotPasswordHandler.HandleAsync(command);
        }

        public async Task<ResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request);
            return await _resetPasswordHandler.HandleAsync(command);
        }
    }
}
