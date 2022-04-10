using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class ResetPasswordRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string ConfirmPassword { get; set; }
        [Required] public string Token { get; set; }
    }
}