using Microsoft.AspNetCore.Identity;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Web;

public static class UserSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        List<(Guid id, string email, string username, string firstName, string middleName, string lastName, DateTime dob, string role)> users =
        [
            (Guid.Parse("e2f4d483-a7fe-4b3c-9d6f-0c24e1234567"), "johndoe@example.com", "johndoe", "John", "A.", "Doae",
                new DateTime(1990, 1, 1), "Director"),
            (Guid.Parse("c7d81a30-6455-4a1f-8f47-923c1234abcd"), "janesmith@example.com", "janesmith", "Jane", "B.",
                "Smith", new DateTime(1985, 5, 15), "Director"),
        ];

        foreach ((Guid id, string email, string username, string firstName, string middleName, string lastName, DateTime dob, string role) in users)
        {
            if (await userManager.FindByEmailAsync(email) != null)
            {
                continue;
            }

            User user = new()
            {
                Id = id,
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                DateOfBirth = dob,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception($"Failed to create user {email}: " +
                                    string.Join(", ", result.Errors));
            }
        }
    }
}
