using System.Collections.Generic;
using System.Threading.Tasks;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Interfaces
{
    public interface IFavoritesService
    {
        void Create(Favorite favorite);
        Task<List<Favorite>> GetAll();

        Task<Favorite> Get(int id);

        void Delete(int id);
    }
}