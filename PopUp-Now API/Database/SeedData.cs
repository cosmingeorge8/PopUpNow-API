using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Database
{
    public static class SeedData
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            SeedRole(roleManager);
            SeedUser(userManager);
        }

        private static void SeedUser(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("SuperUser").Result != null) return;
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
    }
}