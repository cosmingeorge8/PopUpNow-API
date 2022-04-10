using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Services
{
    public class FavoritesService : IFavoritesService
    {
        public readonly DataContext _dataContext;

        public FavoritesService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<List<Favorite>> GetAll()
        {
            return _dataContext.Favorites.ToListAsync();
        }

        public async Task<Favorite> Get(int id)
        {
            var result = await _dataContext.Favorites.FindAsync(id);
            if (result is null)
            {
                throw new Exception("Favorite not found");
            }

            return result;
        }

        public void Delete(int id)
        {
            var favorite = Get(id);
            _dataContext.Remove(favorite);
            _dataContext.SaveChangesAsync();
        }

        public void Create(Favorite favorite)
        {
            var user = favorite.User;
            if (user.IsAlreadyFavorite(favorite.Property))
            {
                throw new Exception("Property already in the favorites list");
            }

            user.AddFavorite(favorite);
            _dataContext.SaveChangesAsync();
        }
    }
}