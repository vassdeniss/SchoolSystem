﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class StudentServiceTests : UnitTestBase
{
    private StudentService _studentService;

    [SetUp]
    public void SetUp()
    {
        this._studentService = new StudentService(this.repo, this.mapper);
    }

    [Test]
    public async Task GetStudentsByClassAsync_ShouldReturnStudentsForClass()
    {
        // Arrange
        Guid classId = this.testDb.Class1.Id;

        // Act
        IEnumerable<StudentDto> result = await this._studentService.GetStudentsByClassAsync(classId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.All(s => s.ClassId == classId));
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetStudentAsync_ShouldReturnStudent_WhenStudentExists()
    {
        // Arrange
        Guid id = this.testDb.Student1.Id;

        // Act
        StudentDto? result = await this._studentService.GetStudentAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetStudentAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        StudentDto? result = await this._studentService.GetStudentAsync(id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void CreateStudentAsync_ShouldThrowException_WhenUserIsAlreadyStudent()
    {
        // Arrange
        StudentDto dto = new()
        {
            UserId = this.testDb.Student1.UserId,
            ClassId = this.testDb.Class1.Id
        };

        // Act & Assert
        Assert.That(async () => await this._studentService.CreateStudentAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("User is already a student."));
    }

    [Test]
    public async Task CreateStudentAsync_ShouldCreateStudent_WhenValid()
    {
        // Arrange
        StudentDto dto = new()
        {
            UserId = Guid.NewGuid(),
            ClassId = this.testDb.Class1.Id
        };
        int studentCountBefore = await this.repo.AllReadonly<Student>().CountAsync();

        // Act
        await this._studentService.CreateStudentAsync(dto);

        // Assert
        int studentCountAfter = await this.repo.AllReadonly<Student>().CountAsync();
        Assert.That(studentCountAfter, Is.EqualTo(studentCountBefore + 1));

        Student? newStudent = await this.repo.AllReadonly<Student>()
            .FirstOrDefaultAsync(s => s.UserId == dto.UserId);
        Assert.That(newStudent, Is.Not.Null);
        Assert.That(newStudent.ClassId, Is.EqualTo(dto.ClassId));
    }

    [Test]
    public async Task UpdateStudentAsync_ShouldUpdateClassId_WhenStudentExists()
    {
        // Arrange
        Student existingStudent = this.testDb.Student1;
        StudentDto dto = new()
        {
            Id = existingStudent.Id,
            ClassId = this.testDb.Class2.Id
        };

        // Act
        await this._studentService.UpdateStudentAsync(dto);

        // Assert
        Student updatedStudent = await this.repo.GetByIdAsync<Student>(existingStudent.Id);
        Assert.That(updatedStudent, Is.Not.Null);
        Assert.That(updatedStudent.ClassId, Is.EqualTo(dto.ClassId));
    }

    [Test]
    public void UpdateStudentAsync_ShouldThrowException_WhenStudentNotFound()
    {
        // Arrange
        StudentDto dto = new()
        {
            Id = Guid.NewGuid(),
            ClassId = this.testDb.Class1.Id
        };

        // Act & Assert
        Assert.That(async () => await this._studentService.UpdateStudentAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("Student not found."));
    }

    [Test]
    public async Task DeleteStudentAsync_ShouldDeleteStudent_WhenStudentExists()
    {
        // Arrange
        Guid id = this.testDb.Student1.Id;

        // Act
        await this._studentService.DeleteStudentAsync(id);

        // Assert
        Student deletedStudent = await this.repo.GetByIdAsync<Student>(id);
        Assert.That(deletedStudent, Is.Null);
    }
}
