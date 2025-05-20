using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetUsersWithRoleAsync(string role);
    Task<UserDto> EditUserAsync(UserDto updatedUserDto);
    Task DeleteUserAsync(Guid userId);
}
