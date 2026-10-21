using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Request.UserRequest;
namespace FunDo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FundooController : ControllerBase
    {
        private IUserBL _userBL;

        public FundooController(IUserBL userBL)
        {
            _userBL = userBL;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationModel registrationModel)
        {
            var response = await _userBL.RegisterUserBL(registrationModel);
            if (!response.Success) 
            { 
                return Conflict(response); 
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
        {
            var response = await _userBL.LoginUserBL(loginModel); 
            if (!response.Success) 
            { 
                return Unauthorized(response); 
            }
            return Ok(response);
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            var response = await _userBL.ForgotPasswordAsync(request);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var response = await _userBL.ResetPasswordAsync(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                Message = "You are authorized!",
                UserId = userId
            });
        }

    }
}

