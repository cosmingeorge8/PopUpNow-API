using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class UserUpdateRequest
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required] public string Name { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string CurrentPassword { get; set; }
    }
}