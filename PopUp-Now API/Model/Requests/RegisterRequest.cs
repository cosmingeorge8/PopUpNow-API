using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        [Required] public string Name { get; set; }
        public bool LandlordRequest { get; set; }

        public string GetUserName()
        {
            return Email.Split('@')[0];
        }
    }
}