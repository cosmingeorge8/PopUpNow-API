using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using SendGrid.Helpers.Errors.Model;

namespace PopUp_Now_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;

        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /**
         * Get all categories
         */
        public async Task<List<Category>> Get()
        {
            var result = await _dataContext.Categories.ToListAsync();
            if (result is null)
            {
                throw new NotFoundException("No category found");
            }

            return result;
        }
    }
}