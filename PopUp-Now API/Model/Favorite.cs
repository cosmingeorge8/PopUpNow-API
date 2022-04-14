using Microsoft.AspNetCore.Identity;

namespace PopUp_Now_API.Model
{
    public class Favorite
    {
        public int Id { get; set; }

        public IdentityUser User { get; set; }

        public Property Property { get; set; }

        public string GetURL()
        {
            return $"/api/favorite{Id}";
        }
    }
}