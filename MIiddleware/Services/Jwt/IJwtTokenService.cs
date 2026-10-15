using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email);
        string GeneratePasswordResetToken(int userId, string email);
        int? ValidatePasswordResetToken(string token);

    }
}
