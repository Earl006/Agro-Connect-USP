using System;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Seeders;

public class RoleSeeder
{

    public static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if(!roleManager.RoleExistsAsync("User").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "User";
            IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        }
        if(!roleManager.RoleExistsAsync("Admin").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "Admin";
            IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        }
    }

}
