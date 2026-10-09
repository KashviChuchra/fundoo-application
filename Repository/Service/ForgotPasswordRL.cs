using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class ForgotPasswordRL : IForgotPasswordRL
    {
        private readonly FundooContext _context;

        public ForgotPasswordRL(FundooContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            email = email.Trim();

            return await _context.Users
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task SaveResetTokenAsync(
            int userId,
            PasswordResetTokenEntity resetToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var existingTokens = await _context.PasswordResetTokens
                .Where(token => token.UserId == userId && !token.IsUsed)
                .ToListAsync();

            foreach (var token in existingTokens)
            {
                token.IsUsed = true;
            }

            resetToken.UserId = userId;

            await _context.PasswordResetTokens.AddAsync(resetToken);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task InvalidateResetTokenAsync(int resetTokenId)
        {
            var token = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(token =>
                    token.ResetTokenId == resetTokenId);

            if (token is not null && !token.IsUsed)
            {
                token.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}