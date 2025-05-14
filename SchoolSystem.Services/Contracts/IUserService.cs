using SchoolSystem.Common;

namespace SchoolSystem.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserWithRoleAsync(Guid userId, string role);
    Task<IEnumerable<UserDto>> GetUsersWithRoleAsync(string role);
}
