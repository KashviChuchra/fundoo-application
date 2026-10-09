using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IForgotPasswordRL
    {
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task SaveResetTokenAsync(int userId, PasswordResetTokenEntity resetToken); 
        Task InvalidateResetTokenAsync(int resetTokenId);
    }
}
