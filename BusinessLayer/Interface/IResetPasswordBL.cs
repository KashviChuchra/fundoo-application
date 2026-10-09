using ModelLayer.Request;
using ModelLayer.Response;

namespace BusinessLayer.Interface
{
    public interface IResetPasswordBL
    {
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}