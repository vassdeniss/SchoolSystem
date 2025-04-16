﻿using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Attendance
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid StudentId { get; init; }
    public Student Student { get; init; } = null!;

    public Guid SubjectId { get; init; }
    public Subject Subject { get; init; } = null!;

    [Required]
    [MaxLength(50)]
    public string AbsenceType { get; init; } = null!;

    [Required]
    [MaxLength(50)]
    public string Status { get; init; } = null!;
}

