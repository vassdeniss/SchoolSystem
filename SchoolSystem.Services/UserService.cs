using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class UserService(UserManager<User> userManager, ILogger<UserService> logger, IMapper mapper) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        logger.LogInformation("Fetching all users.");

        List<User> users = await userManager.Users.ToListAsync();

        List<UserDto> usersWithRoles = [];
        foreach (User user in users)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            UserDto? userDto = mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();
            usersWithRoles.Add(userDto);
        }

        logger.LogInformation("Fetched {UserCount} users.", usersWithRoles.Count);

        return usersWithRoles;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        logger.LogInformation("Fetching user with ID: {UserId}", userId);

        User? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            logger.LogWarning("User {UserId} not found", userId);
            throw new InvalidOperationException("User not found.");
        }

        UserDto? userDto = mapper.Map<UserDto>(user);
        userDto.Roles = await userManager.GetRolesAsync(user);

        logger.LogInformation("Successfully fetched user {UserId}", userId);
        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetUsersWithRoleAsync(string role)
    {
        logger.LogInformation("Fetching users in role: {Role}", role);

        IList<User> usersInRole = await userManager.GetUsersInRoleAsync(role);
        List<UserDto> result = usersInRole.Select(mapper.Map<UserDto>).ToList();

        logger.LogInformation("Found {Count} users in role {Role}", result.Count, role);
        return result;
    }

    public async Task<UserDto> EditUserAsync(UserDto updatedUserDto)
    {
        logger.LogInformation("Attempting to update user {UserId}", updatedUserDto.Id);

        // Find user by ID
        User? user = await userManager.FindByIdAsync(updatedUserDto.Id.ToString());
        if (user == null)
        {
            logger.LogWarning("User {UserId} not found for update", updatedUserDto.Id);
            throw new InvalidOperationException("User not found.");
        }

        // Map editable fields from DTO to the existing user entity
        mapper.Map(updatedUserDto, user);
        logger.LogDebug("Mapped updated fields to user {UserId}", updatedUserDto.Id);

        // Attempt to update the user in the identity store
        IdentityResult updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            logger.LogError("Failed to update user {UserId}: {Errors}", updatedUserDto.Id,
                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            throw new InvalidOperationException("Failed to update user.");
        }

        logger.LogInformation("User {UserId} updated successfully", updatedUserDto.Id);

        UserDto result = mapper.Map<UserDto>(user);
        result.Roles = await userManager.GetRolesAsync(user);
        return result;
    }
    
    public async Task DeleteUserAsync(Guid userId)
    {
        logger.LogInformation("Attempting to delete user {UserId}", userId);

        User? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            logger.LogWarning("User {UserId} not found for deletion", userId);
            throw new InvalidOperationException("User not found.");
        }

        IdentityResult deleteResult = await userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            logger.LogError("Failed to delete user {UserId}: {Errors}", userId,
                string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
            throw new InvalidOperationException("Failed to delete user");
        }

        logger.LogInformation("User {UserId} deleted successfully", userId);
    }
}
