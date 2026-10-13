using ModelLayer.Request.UserRequest;
using ModelLayer.Response;

namespace BusinessLayer.Interface
{
    public interface IResetPasswordBL
    {
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}