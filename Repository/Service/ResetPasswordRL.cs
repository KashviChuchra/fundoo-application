using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class ResetPasswordRL : IResetPasswordRL
    {
        private readonly FundooContext _fundooContext;

        public ResetPasswordRL(FundooContext fundooContext)
        {
            _fundooContext = fundooContext;
        }

        public async Task<bool> ResetPasswordAsync(string tokenHash,string newPasswordHash)
        {
            var currentTime = DateTime.UtcNow;

            var resetToken = await _fundooContext.PasswordResetTokens
                .Include(token => token.User)
                .FirstOrDefaultAsync(token =>
                    token.TokenHash == tokenHash &&
                    !token.IsUsed &&
                    token.ExpiryTime > currentTime);

            if (resetToken == null)
            {
                return false;
            }

            resetToken.User.Password = newPasswordHash;
            resetToken.IsUsed = true;

            await _fundooContext.SaveChangesAsync();

            return true;
        }
    }
}
