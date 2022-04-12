using System.Linq;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Database
{
    public static class SeedData
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,
            DataContext dataContext)
        {
            SeedRole(roleManager);
            SeedUser(userManager);
            SeedCategories(dataContext);
        }

        private static void SeedUser(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("SuperUser").Result == null)
            {
                var user = new User()
                {
                    UserName = "SuperUser",
                    Email = "superuser@localhost",
                    Name = "SuperUser"
                };
                var result = userManager.CreateAsync(user, "H3ngelo2020!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }

            if (userManager.FindByNameAsync("Landlord").Result != null) return;
            var landlord = new User()
            {
                UserName = "Landlord",
                Email = "landlord@email.com",
                Name = "Landlord"
            };
            var res = userManager.CreateAsync(landlord, "H3ngelo2020!").Result;

            if (res.Succeeded)
            {
                userManager.AddToRoleAsync(landlord, "Landlord").Wait();
            }
        }


        private static void SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }

            if (!roleManager.RoleExistsAsync("Landlord").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Landlord")).Wait();
            }
        }


        private static void SeedCategories(DataContext dataContext)
        {
            var categories = dataContext.Categories.Count();
            if (categories > 0) return;
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

            dataContext.Categories.AddRangeAsync(defaultCategories).Wait();
            dataContext.SaveChangesAsync().Wait();
        }

        public static readonly string CategoryShopsImgPath = "/Upload/categoryImages/shops.jpg";
        public static readonly string CategoryMarketsImgPath = "/Upload/categoryImages/markets.jpeg";
        public static readonly string CategoryShoppingCentersImgPath = "/Upload/categoryImages/shopping_centers.jpeg";
    }
}