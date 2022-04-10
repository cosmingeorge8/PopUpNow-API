using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;

        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
            InitDefault();
        }

        private void InitDefault()
        {
            var categories = Get();
            if (categories.Result.Count > 0) return;
            var defaultCategories = new[]
            {
                new Category
                {
                    Name = "Shops",
                    Description = "Find the best places to open a store",
                    ImagePath = CategoryShopsImgPath
                },
                new Category
                {
                    Name = "Markets",
                    Description = "Find the best places to open a market",
                    ImagePath = CategoryMarketsImgPath
                },
                new Category
                {
                    Name = "Shopping Centers",
                    Description = "Find the best places to open a shopping center",
                    ImagePath = CategoryShoppingCentersImgPath
                }
            };

            _dataContext.Categories.AddRange(defaultCategories);
            _dataContext.SaveChanges();
        }

        public async Task<List<Category>> Get()
        {
            return await _dataContext.Categories.ToListAsync();
        }

        private static readonly string CategoryShopsImgPath = "/Upload/categoryImages/shops.jpg";
        private static readonly string CategoryMarketsImgPath = "/Upload/categoryImages/markets.jpeg";
        private static readonly string CategoryShoppingCentersImgPath = "/Upload/categoryImages/shopping_centers.jpeg";
    }
}