using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class AssistanceRequest
    {
        [Required] public string Email { get; set; }

        [Required] public string Message { get; set; }

        public Email GetEmail()
        {
            return new Email
            {
                Subject = "Assistance" + Email,
                Body = Message,
                ToEmail = "popup@assistance.now"
            };
        }
    }
}