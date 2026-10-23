using BusinessLayer.User.Command.HandlerInterface;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Middleware.Services.Email;
using Middleware.Services.Jwt;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BusinessLayer.User.Command
{
    public class ForgotPasswordHandler: IForgotPasswordHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordHandler> _logger;

        public ForgotPasswordHandler(IUserRL userRL, IJwtTokenService jwtTokenService, IEmailSender emailSender, IConfiguration configuration,
            ILogger<ForgotPasswordHandler> logger)
        {
            _userRL = userRL;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ResponseModel<string>> HandleAsync(ForgotPasswordCommand command)
        {
            const string genericMessage = "If an account exists for this email, password reset instructions will be sent.";
            var request = command.Request;

            if (request == null || string.IsNullOrWhiteSpace(request.Email))
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Email is required."
                };
            }

            try
            {
                var user = await _userRL.GetUserByEmailAsync(request.Email.Trim());

                if (user == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = true,
                        Message = genericMessage
                    };
                }

                var token = _jwtTokenService.GeneratePasswordResetToken(user.UserId, user.Email);

                var frontendUrl = _configuration["PasswordReset:FrontendUrl"];

                if (string.IsNullOrWhiteSpace(frontendUrl))
                {
                    throw new InvalidOperationException("Password reset frontend URL is not configured.");
                }

                var resetLink = QueryHelpers.AddQueryString(frontendUrl, "token", token);

                var emailBody =
                    $"Use this link to reset your Fundoo password:\n\n{resetLink}\n\n" +
                    "This link expires in 15 minutes. " +
                    "If you did not request a password reset, ignore this email.";


                await _emailSender.SendEmailAsync(user.Email, "Fundoo Password Reset", emailBody);

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
    }

}