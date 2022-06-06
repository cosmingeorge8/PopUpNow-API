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

        /**
         * Method takes in an assistance request and composes an email that will be sent to the server administrator
         * A 400 error will be shown if 
         */
        [HttpPost]
        public async Task<IActionResult> GetAssistance(AssistanceRequest assistanceRequest)
        {
            await _mailService.SendEmailAsync(assistanceRequest.GetEmail());
            return Ok("Email was sent");
        }
    }
}