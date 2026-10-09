using BusinessLayer.Interface;
using ModelLayer.Request;
using ModelLayer.Response;
using RepositoryLayer.Interface;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Service
{
    public class ResetPasswordBL : IResetPasswordBL
    {
        private readonly IResetPasswordRL _resetPasswordRL;

        public ResetPasswordBL(IResetPasswordRL resetPasswordRL)
        {
            _resetPasswordRL = resetPasswordRL;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(
            ResetPasswordRequest request)
        {
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.Token)));

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);

            var isResetSuccessful =
                await _resetPasswordRL.ResetPasswordAsync(
                    tokenHash,
                    newPasswordHash);

            if (!isResetSuccessful)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "The reset link is invalid, expired, or has already been used."
                };
            }

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "Password has been reset successfully."
            };
        }
    }
}