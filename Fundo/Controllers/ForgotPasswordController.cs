
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Request.UserRequest;
using System.Net.NetworkInformation;

namespace FunDo.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    // Tells Swagger that every endpoint in this controller returns JSON. It shows up in the UI as the response media type.
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordBL _forgotPasswordBL;

        public ForgotPasswordController(
            IForgotPasswordBL forgotPasswordBL)
        {
            _forgotPasswordBL = forgotPasswordBL;
        }

        /// <summary>
        ///  These are XML comments. 
        ///  Other XML comments - <param>, <returns>, <response>
        ///  These are human-readable descriptions. Swagger UI reads them and displays them next to the endpoint
        ///     <summary> is the short description shown beside the endpoint
        ///     <param> describes the request body.
        ///     <returns> describes what comes back.
        ///     <response code = "..." > adds a description to each status code listed in the UI.
        ///  -> For these to appear, XML documentation must be enabled in your .csproj 
        ///  -> and wired into Swashbuckle (see the setup in my earlier message). 
        ///  -> Without that, the comments are only visible in your code editor.
        /// </summary>

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(
            [FromBody] ForgotPasswordRequest request)
        {
            var response = await _forgotPasswordBL.ForgotPasswordAsync(request);

            return Ok(response);
        }
    }
}
