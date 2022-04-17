﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        /**
         * Get a list of favorite objects for a specific user
         */
        public Task<List<Favorite>> GetAll(User user)
        {
            return _dataContext.Favorites
                .Where(favorite => favorite.User.Id.Equals(user.Id))
                .Include(favorite => favorite.Property)
                .ToListAsync();
        }

        /**
         * Get favorite object by id
         */
        public async Task<Favorite> Get(int id)
        {
            var result = await _dataContext.Favorites.FindAsync(id);
            if (result is null)
            {
                throw new Exception("Favorite not found");
            }

            return result;
        }

        /**
         * Get the favorite object by user and property
         */
        private async Task<Favorite> Get(User user, Property property)
        {
            return await _dataContext.Favorites
                .Where(favorite => favorite.User.Id.Equals(user.Id)
                                   && favorite.Property.Id.Equals(property.Id))
                .FirstOrDefaultAsync();
        }

        /**
         * Delete a favorite object
         */
        public async Task<Favorite> Delete(User user, Property property)
        {
            var favorite = await Get(user, property);
            if (favorite is null)
            {
                throw new Exception("favorite not found");
            }

            Delete(favorite);
            return favorite;
        }

        /**
         * Delete a favorite object
         */
        private void Delete(Favorite favorite)
        {
            _dataContext.Remove(favorite);
            _dataContext.SaveChangesAsync();
        }

        /**
         * Create a favorite object
         */
        public async Task Create(Favorite favorite)
        {
            await _dataContext.Favorites.AddAsync(favorite);
            await _dataContext.SaveChangesAsync();
        }
    }
}