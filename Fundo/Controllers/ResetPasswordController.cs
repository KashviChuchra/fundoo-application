using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Request;

namespace FunDo.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IResetPasswordBL _resetPasswordBL;

        public ResetPasswordController(IResetPasswordBL resetPasswordBL)
        {
            _resetPasswordBL = resetPasswordBL;
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromBody] ResetPasswordRequest request)
        {
            var response = await _resetPasswordBL.ResetPasswordAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
