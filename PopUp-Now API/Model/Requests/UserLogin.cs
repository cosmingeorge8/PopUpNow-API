using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    /**
 * @author Mucalau Cosmin
 *
 * The class's purpose is to encapsulate the user authentication attempts
 */
    public class UserLogin
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}