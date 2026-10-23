using BusinessLayer.User.Command.HandlerInterface;
using Microsoft.Extensions.Logging;
using Middleware.Services.Hashing;
using Middleware.Services.Jwt;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.User.Command
{
    public class ResetPasswordHandler: IResetPasswordHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly CustomHashing _customHasher;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(IUserRL userRL, IJwtTokenService jwtTokenService, CustomHashing customHasher,ILogger<ResetPasswordHandler> logger)
        {
            _userRL = userRL;
            _jwtTokenService = jwtTokenService;
            _customHasher = customHasher;
            _logger = logger;
        }

        public async Task<ResponseModel<string>> HandleAsync(ResetPasswordCommand command)
        {
            var request = command.Request;

            if (request == null || string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Token and new password are required."
                };
            }

            var userId = _jwtTokenService.ValidatePasswordResetToken(request.Token);

            if (userId == null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "The reset link is invalid or expired."
                };
            }

            try
            {
                //var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                var passwordHash = _customHasher.HashPassword(request.NewPassword);

                var updated = await _userRL.UpdatePasswordAsync(userId.Value,passwordHash);

                if (!updated)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unable to reset password."
                    };
                }

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Password has been reset successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to reset the user's password.");

                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unable to reset password."
                };
            }
        }
    }
}
