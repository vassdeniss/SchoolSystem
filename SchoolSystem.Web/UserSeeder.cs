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
            (Guid.Parse("97c18abd-5743-4173-8f66-cd43363e55d5"), "somestudent1@example.com", "somestudent1", "Some", "S.",
                "Student1", new DateTime(2003, 5, 15), "Student"),
            (Guid.Parse("bdcc8dcc-4d8e-4c97-a576-3aee878059c0"), "somestudent2@example.com", "somestudent2", "Mome", "B.",
                "Student2", new DateTime(2003, 7, 4), "Student"),
            (Guid.Parse("1396ac5f-745d-47b4-8cef-21a0e7e32bd9"), "someteacher1@example.com", "someteacher1", "Some", "B.",
                "Teacher1", new DateTime(2003, 7, 4), "Teacher"),
            (Guid.Parse("7f74d5cd-5061-4e53-a10b-221cfb9488a0"), "someteacher2@example.com", "someteacher2", "Some", "B.",
                "Teacher2", new DateTime(2003, 7, 4), "Teacher"),
            (Guid.Parse("8f438533-ab99-4235-ab40-f2bc7d2b96ba"), "someteacher3@example.com", "someteacher3", "Some", "B.",
                "Teacher3", new DateTime(2003, 7, 4), "Teacher")
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
