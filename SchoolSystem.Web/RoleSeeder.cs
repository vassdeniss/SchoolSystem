using Microsoft.AspNetCore.Identity;

namespace SchoolSystem.Web;

public static class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roleNames = { "Administrator", "Director", "Teacher", "Parent", "Student" };
        foreach (string roleName in roleNames)
        {
            bool roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }
}