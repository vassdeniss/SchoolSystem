namespace SchoolSystem.Services.Exceptions;

public class UserNotFoundException(Guid userId) 
    : Exception($"User with ID {userId} not found");

public class InvalidUserRoleException(string role) 
    : Exception($"User is not a {role}");
