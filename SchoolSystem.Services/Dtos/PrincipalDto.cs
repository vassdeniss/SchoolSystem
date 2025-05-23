﻿using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class PrincipalDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;
    
    public School? School { get; init; }

    public string Specialization { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;
}
