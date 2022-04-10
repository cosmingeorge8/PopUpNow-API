using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace PopUp_Now_API.Model
{
    public class User : IdentityUser
    {
        public Collection<Favorite> Favorites { get; set; }
        public string Name { get; set; }

        public bool IsAlreadyFavorite(Property property)
        {
            foreach (var favorite in Favorites)
            {
                if (favorite.Property == property)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddFavorite(Favorite favorite)
        {
            Favorites.Add(favorite);
        }
    }
}