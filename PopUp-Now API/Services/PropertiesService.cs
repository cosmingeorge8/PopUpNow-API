﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Exceptions;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;
using static System.String;

namespace PopUp_Now_API.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly DataContext _dataContext;

        private readonly IUsersService _usersService;

        public PropertiesService(DataContext dataContext, IUsersService usersService)
        {
            _dataContext = dataContext;
            _usersService = usersService;
        }

        /**
         * Get a list of all properties
         */
        public Task<List<Property>> GetAll()
        {
            return _dataContext.Properties
                .Include(property => property.location)
                .Include(property => property.User)
                .Include(property => property.Image)
                .Include(property => property.detailImages)
                .Include(property => property.Price)
                .Include(property => property.Category)
                .ToListAsync();
        }

        public async Task<Property> Get(int id)
        {
            var property = await _dataContext.Properties
                .Include(element => element.User)
                .Where(element => element.Id.Equals(id))
                .FirstAsync();
            if (property is null)
            {
                throw new PropertiesException($"Property with {id} not found");
            }

            return property;
        }

        /**
         * Delete a property by id
         */
        public async Task<Property> Delete(int id)
        {
            var property = await Get(id);
            if (property is null)
            {
                throw new Exception("Property not found ");
            }

            return _dataContext.Properties.Remove(property).Entity;
        }
        
        /**
         * Add a new property
         */
        public async Task<Property> Add(PropertyRequest propertyRequest, string email)
        {
            var landlord = await _usersService.GetUser(email);
            var property = new Property
            {
                detailImages = propertyRequest.Images,
                location = propertyRequest.Location,
                MinimumBookingDays = propertyRequest.MinimumBookingDays,
                Name = propertyRequest.Name,
                Price = propertyRequest.Price,
                Size = propertyRequest.Size,
                User = landlord,
                Category = _dataContext.Categories.FirstOrDefault(cat => cat.Id == propertyRequest.Category.Id),
                Description = propertyRequest.Description,
                Image = propertyRequest.Image,
            };

            await _dataContext.Properties.AddAsync(property);
            await _dataContext.SaveChangesAsync();

            return property;
        }

        /**
         * Update a property
         */
        public async Task<bool> Update(PropertyRequest propertyRequest)
        {
            var property = await Get(propertyRequest.Id);

            property.Update(propertyRequest);

            var result = _dataContext.Properties.Update(property);

            if (result is null)
            {
                return false;
            }

            return true;
        }

        /**
         * Get a list of all properties by category
         */
        public async Task<List<Property>> GetByCategory(int categoryId)
        {
            var category = await _dataContext.Categories.FindAsync(categoryId);

            if (category is null)
            {
                throw new PropertiesException("No category with the given id was found");
            }

            return await _dataContext.Properties
                .Where(property =>
                    property.Category.Id == categoryId)
                .Include(property => property.location)
                .Include(property => property.User)
                .Include(property => property.Image)
                .Include(property => property.detailImages)
                .Include(property => property.Price)
                .Include(property => property.Category)
                .ToListAsync();
        }

        public async Task<List<Property>> GetByLandlord(string email)
        {
            var user = await _usersService.GetUser(email);
            return await _dataContext.Properties
                .Where(property => property.User.Id.Equals(user.Id))
                .Include(property => property.location)
                .Include(property => property.User)
                .Include(property => property.Image)
                .Include(property => property.detailImages)
                .Include(property => property.Price)
                .Include(property => property.Category)
                .ToListAsync();
        }


        /**
     * If the string is empty, an exception will be thrown
     *
     * Otherwise a search will be performed, returning the records that have a matching Name or Description
     */
        public Task<List<Property>> GetAll(string query)
        {
            if (IsNullOrEmpty(query))
            {
                throw new PropertiesException("Search query was empty");
            }

            var lowerQuery = query.ToLower();

            return _dataContext.Properties
                .Where(property =>
                    property.Name.ToLower().Contains(lowerQuery) ||
                    property.Description.ToLower().Contains(lowerQuery) ||
                    property.location.ToString().ToLower().Contains(lowerQuery))
                .ToListAsync();
        }
    }
}