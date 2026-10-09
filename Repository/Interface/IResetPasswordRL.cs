using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IResetPasswordRL
    {
        Task<bool> ResetPasswordAsync(string tokenHash, string newPasswordHash);
    }
}
