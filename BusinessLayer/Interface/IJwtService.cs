using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email);
    }
}
