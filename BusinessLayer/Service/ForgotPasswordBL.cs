using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BusinessLayer.Interface;
using ModelLayer.Request;
using ModelLayer.Response;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class ForgotPasswordBL : IForgotPasswordBL
    {
        private readonly IForgotPasswordRL _forgotPasswordRL;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordBL> _logger;

        public ForgotPasswordBL(
            IForgotPasswordRL forgotPasswordRL,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<ForgotPasswordBL> logger)
        {
            _forgotPasswordRL = forgotPasswordRL;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(
            ForgotPasswordRequest model)
        {
            const string genericMessage =
                "If an account exists for this email, password reset instructions will be sent.";

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "Email is required."
                };
            }

            PasswordResetTokenEntity? savedToken = null;

            try
            {
                var user = await _forgotPasswordRL.GetUserByEmailAsync(
                    model.Email.Trim());

                // Never reveal whether the account exists.
                if (user is null)
                {
                    return new ForgotPasswordResponse
                    {
                        Success = true,
                        Message = genericMessage
                    };
                }

                // Generate a cryptographically secure random token.
                var rawToken = WebEncoders.Base64UrlEncode(
                    RandomNumberGenerator.GetBytes(32));

                // Store only the SHA-256 hash in the database.
                var tokenHash = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

                savedToken = new PasswordResetTokenEntity
                {
                    UserId = user.UserId,
                    TokenHash = tokenHash,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(20),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _forgotPasswordRL.SaveResetTokenAsync(
                    user.UserId, savedToken);

                // Build the reset link using a configured frontend URL.
                var resetPageUrl = _configuration[
                    "PasswordReset:FrontendUrl"];

                var messagingUrl = _configuration[
                    "MessagingService:SendEmailUrl"];

                if (string.IsNullOrWhiteSpace(resetPageUrl) ||
                    string.IsNullOrWhiteSpace(messagingUrl))
                {
                    throw new InvalidOperationException(
                        "Password reset or messaging URL is not configured.");
                }

                var resetLink = QueryHelpers.AddQueryString(
                    resetPageUrl,
                    "token",
                    rawToken);

                var emailRequest = new
                {
                    ToEmail = user.Email,
                    Subject = "Fundoo Password Reset",
                    Body = $"""
                        Hello,

                        We received a request to reset your Fundoo password.

                        Use this link to reset your password:
                        {resetLink}

                        This link expires in 20 minutes and can only be used once.
                        If you did not request this, you can ignore this email.
                        """
                };

                var httpClient = _httpClientFactory.CreateClient(
                    "MessagingService");

                using var response = await httpClient.PostAsJsonAsync(
                    messagingUrl,
                    emailRequest);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"Messaging Service returned HTTP {(int)response.StatusCode}.");
                }

                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = genericMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Forgot password processing failed.");

                // If the email could not be sent, disable the new token
                // so it cannot be used without the user receiving the link.
                if (savedToken is not null)
                {
                    try
                    {
                        await _forgotPasswordRL.InvalidateResetTokenAsync(
                            savedToken.ResetTokenId);
                    }
                    catch (Exception cleanupException)
                    {
                        _logger.LogError(
                            cleanupException,
                            "Failed to invalidate a reset token after an error.");
                    }
                }

                // Keep the public response generic to avoid account enumeration
                // and exposing internal implementation details.
                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = genericMessage
                };
            }
        }
    }
}