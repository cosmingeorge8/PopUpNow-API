using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Interfaces
{
    public interface IFavoritesService
    {
        Task Create(Favorite favorite);
        Task<List<Favorite>> GetAll(User user);

        Task<Favorite> Get(int id);

        Task<Favorite> Delete(User user, Property property);
    }
}