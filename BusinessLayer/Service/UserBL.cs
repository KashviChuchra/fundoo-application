
using BCrypt.Net;
using BusinessLayer.Interface;
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

        public UserBL(
            IUserRL userRL,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILogger<UserBL> logger,
            CustomHashing customHasher)
        {
            _userRL = userRL;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
            _customHasher = customHasher;
        }

        public async Task<ResponseModel<string>> RegisterUserBL(
            RegistrationModel registrationModel)
        {
            registrationModel.Password=_customHasher.HashPassword(registrationModel.Password);
            return await _userRL.RegisterUserRL(registrationModel);
        }

        public async Task<ResponseModel<LoginResponseModel>> LoginUserBL(
            LoginModel loginModel)
        {
            var response = await _userRL.LoginUserRL(loginModel);

            if (!response.Success || response.Data == null)
                return response;

            var user = response.Data;

            user.Token = _jwtTokenService.GenerateToken(
                user.UserId, user.Email);

            return response;
        }

        public async Task<ResponseModel<string>> ForgotPasswordAsync(
            ForgotPasswordRequest request)
        {
            const string genericMessage =
                "If an account exists for this email, password reset instructions will be sent.";

            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Email is required."
                };
            }

            try
            {
                var user = await _userRL.GetUserByEmailAsync(
                    request.Email.Trim());

                if (user == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = true,
                        Message = genericMessage
                    };
                }

                var token = _jwtTokenService.GeneratePasswordResetToken(
                    user.UserId, user.Email);

                var frontendUrl =
                    _configuration["PasswordReset:FrontendUrl"];

                if (string.IsNullOrWhiteSpace(frontendUrl))
                {
                    throw new InvalidOperationException(
                        "Password reset frontend URL is not configured.");
                }

                var resetLink = QueryHelpers.AddQueryString(
                    frontendUrl, "token", token);

                var emailBody =
                    $"Use this link to reset your Fundoo password:\n\n{resetLink}\n\n" +
                    "This link expires in 15 minutes. If you did not request a password reset, ignore this email.";

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Fundoo Password Reset",
                    emailBody);

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = genericMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process a password-reset request.");

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = genericMessage
                };
            }
        }

        public async Task<ResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Token and new password are required."
                };
            }

            var userId = _jwtTokenService.ValidatePasswordResetToken(
                request.Token);

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

                var updated = await _userRL.UpdatePasswordAsync(userId.Value, passwordHash);

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
                _logger.LogError(ex, "Failed to reset the user's password.");

                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unable to reset password."
                };
            }
        }
    }
}
