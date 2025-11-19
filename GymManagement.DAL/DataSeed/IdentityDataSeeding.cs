using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeed
{
    public static class IdentityDataSeeding
    {
        public static async Task<bool> SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                bool HasUsers = await userManager.Users.AnyAsync();
                bool HasRoles = await roleManager.Roles.AnyAsync();

                if (HasUsers && HasRoles) return false;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole(){Name = "SuperAdmin"},
                        new IdentityRole(){Name = "Admin"}
                    };

                    foreach (var Role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(Role.Name!))
                        {
                            await roleManager.CreateAsync(Role);
                        }
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Yehia",
                        LastName = "NourEldin",
                        UserName = "YehiaNourEldin",
                        Email = "YehiaNourEldin@gmail.com",
                        PhoneNumber = "01123652635"
                    };

                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "Mohamed",
                        UserName = "OmarMohamed",
                        Email = "OmarMohamed@gmail.com",
                        PhoneNumber = "01232589652"
                    };

                    await userManager.CreateAsync(Admin01, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin01, "Admin");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }

    }
}
