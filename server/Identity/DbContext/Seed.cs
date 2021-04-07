using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.DbContext
{
    public class Seed
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@gmail.com",
                        IsVegan = true
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@gmail.com",
                        IsVegan = false
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@gmail.com",
                        IsVegan = true
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}
