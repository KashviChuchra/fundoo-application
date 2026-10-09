using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Request;
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
        

        [HttpPost]
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

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok("You are authorized!");
        }

    }
}

