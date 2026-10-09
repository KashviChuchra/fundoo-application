using MessagingService_BusinessLayer.Interface;
using MessagingService_ModelLayer;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailBL _emailBL;

        public EmailController(IEmailBL emailBL)
        {
            _emailBL = emailBL;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromBody] EmailModel emailModel)
        {
            await _emailBL.SendEmailAsync(emailModel); 

            return Ok(new
            {
                Success = true,
                Message = "Email sent successfully."
            });
        }
    }
}
