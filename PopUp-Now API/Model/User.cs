using Microsoft.AspNetCore.Identity;

namespace PopUp_Now_API.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Image { get; set; }
    }
}