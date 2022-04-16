using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class AssistanceController : ControllerBase
    {
        private readonly IMailService _mailService;

        public AssistanceController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAssistance(AssistanceRequest assistanceRequest)
        {
            try
            {
                await _mailService.SendEmailAsync(assistanceRequest.GetEmail());
                return Ok("Email was sent");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}