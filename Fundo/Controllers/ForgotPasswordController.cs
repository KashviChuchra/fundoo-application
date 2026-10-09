
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Request;

namespace FunDo.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordBL _forgotPasswordBL;

        public ForgotPasswordController(
            IForgotPasswordBL forgotPasswordBL)
        {
            _forgotPasswordBL = forgotPasswordBL;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(
            [FromBody] ForgotPasswordRequest request)
        {
            var response = await _forgotPasswordBL.ForgotPasswordAsync(request);

            return Ok(response);
        }
    }
}
