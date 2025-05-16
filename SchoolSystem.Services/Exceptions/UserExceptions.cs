using Microsoft.AspNetCore.Identity;

namespace SchoolSystem.Services.Exceptions;

public class UserNotFoundException(Guid userId) 
    : Exception($"User with ID {userId} not found");

public class InvalidUserRoleException(string role) 
    : Exception($"User is not a {role}");

public class IdentityOperationException(string message, IEnumerable<IdentityError> errors)
    : Exception($"{message}: {string.Join("; ", errors.Select(e => e.Description))}");
