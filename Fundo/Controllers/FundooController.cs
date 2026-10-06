using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer;
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
        public IActionResult RegisterUser(RegistrationModel registrationModel)
        {
            _userBL.RegisterUserBL(registrationModel);
            return Ok("Registration successful");
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginModel loginModel)
        {
            var result=_userBL.LoginUserBL(loginModel);
            return Ok(result);
        }

    }
}

