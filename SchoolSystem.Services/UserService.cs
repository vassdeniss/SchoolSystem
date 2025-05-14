using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Exceptions;

namespace SchoolSystem.Services;

public class UserService(UserManager<User> userManager, ILogger<UserService> logger, IMapper mapper) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        List<User> users = await userManager.Users.ToListAsync();
        
        List<UserDto> usersWithRoles = [];
        foreach (User user in users)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            UserDto? userDto = mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();
            usersWithRoles.Add(userDto);
        }

        return usersWithRoles;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        User? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            logger.LogWarning("User {UserId} not found", userId);
            throw new UserNotFoundException(userId);
        }
        
        UserDto? userDto = mapper.Map<UserDto>(user);
        userDto.Roles = await userManager.GetRolesAsync(user);
        return userDto;
    }

    public async Task<UserDto> GetUserWithRoleAsync(Guid userId, string role)
    {
        User? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            logger.LogWarning("User {UserId} not found", userId);
            throw new UserNotFoundException(userId);
        }

        bool isInRole = await userManager.IsInRoleAsync(user, role);
        if (!isInRole)
        {
            throw new InvalidUserRoleException(role);
        }

        return mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsersWithRoleAsync(string role)
    {
        IList<User> usersInRole = await userManager.GetUsersInRoleAsync(role);
        return usersInRole.Select(mapper.Map<UserDto>).ToList();
    }
}
