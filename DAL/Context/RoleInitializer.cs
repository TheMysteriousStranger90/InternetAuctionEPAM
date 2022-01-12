using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Context
{
    /// <summary>
    /// Initializing data at startup
    /// </summary>
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "adminexample@gmail.com";
            string adminPassword = "Adminexample123";
            string adminFirstName = "Rodion";
            string adminLastName = "Raskolnikov";
            string userEmail = "levmyshkinl@gmail.com";
            string userPassword = "Myshkin0101";
            string userName = "Lev";
            string userSurname = "Myshkin";
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    Role = "Admin"
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FirstName = userName,
                    LastName = userSurname,
                    Role = "User"
                };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
