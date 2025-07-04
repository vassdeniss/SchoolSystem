using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class CurriculumServiceTestBase : UnitTestBase
{
    protected CurriculumService _curriculumService;

    [SetUp]
    public void SetupCurriculumService()
    {
        this._curriculumService = new CurriculumService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetCurriculumsByClassIdAsyncTests : CurriculumServiceTestBase
{
    [Test]
    public async Task ShouldReturnCurriculumsForValidClass()
    {
        // Arrange
        var classId = this.testDb.Class1.Id;

        // Act
        var result = await this._curriculumService.GetCurriculumsByClassIdAsync(classId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Count(), Is.EqualTo(1), "Expected one curriculum entry for the class");

            var first = result.First();
            Assert.That(first.DayOfWeek, Is.EqualTo("Monday"), "Expected curriculum scheduled on Monday");
            Assert.That(first.StartTime, Is.EqualTo(new TimeSpan(8, 30, 0)), "Start time should match");
            Assert.That(first.EndTime, Is.EqualTo(new TimeSpan(9, 15, 0)), "End time should match");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenClassHasNoCurriculums()
    {
        // Arrange
        var unusedClassId = this.testDb.Class2.Id;

        // Act
        var result = await this._curriculumService.GetCurriculumsByClassIdAsync(unusedClassId);

        // Assert
        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result, Is.Empty, "Expected no curriculums for this class");
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenClassIdDoesNotExist()
    {
        // Arrange
        var invalidClassId = Guid.NewGuid();

        // Act
        var result = await this._curriculumService.GetCurriculumsByClassIdAsync(invalidClassId);

        // Assert
        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result, Is.Empty, "Expected empty list for non-existent class ID");
    }

    [Test]
    public void ShouldThrowException_WhenClassIdIsEmptyGuid()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._curriculumService.GetCurriculumsByClassIdAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Provided class ID cannot be empty"));
    }
}

[TestFixture]
public class GetCurriculumByIdAsyncTests : CurriculumServiceTestBase
{
    [Test]
    public async Task ShouldReturnCurriculum_WhenValidIdProvided()
    {
        // Arrange
        var curriculumId = this.testDb.Curriculum1.Id;

        // Act
        var result = await this._curriculumService.GetCurriculumByIdAsync(curriculumId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Expected curriculum to be found");
            Assert.That(result!.Id, Is.EqualTo(curriculumId), "Curriculum ID should match");
            Assert.That(result.DayOfWeek, Is.EqualTo(this.testDb.Curriculum1.DayOfWeek), "DayOfWeek should match");
            Assert.That(result.StartTime, Is.EqualTo(this.testDb.Curriculum1.StartTime), "StartTime should match");
            Assert.That(result.EndTime, Is.EqualTo(this.testDb.Curriculum1.EndTime), "EndTime should match");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenCurriculumIdDoesNotExist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await this._curriculumService.GetCurriculumByIdAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null, "Expected null when curriculum ID is not found");
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._curriculumService.GetCurriculumByIdAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Curriculum ID cannot be empty"));
    }
}

[TestFixture]
public class CreateCurriculumAsyncTests : CurriculumServiceTestBase
{
    [Test]
    public async Task ShouldCreateCurriculum_WhenDtoIsValid()
    {
        // Arrange
        var dto = new CurriculumDto
        {
            Id = Guid.NewGuid(),
            DayOfWeek = "Thursday",
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(9, 45, 0),
            TeacherId = this.testDb.Teacher1.Id,
            SubjectId = this.testDb.Subject1.Id,
            Class = this.testDb.Class1,
            Teacher = this.testDb.Teacher1,
            Subject = this.testDb.Subject1
        };

        // Act
        await this._curriculumService.CreateCurriculumAsync(dto);

        // Assert
        var created = await this.repo.GetByIdAsync<Curriculum>(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null, "Curriculum should be created");
            Assert.That(created!.DayOfWeek, Is.EqualTo(dto.DayOfWeek), "DayOfWeek should match");
            Assert.That(created.StartTime, Is.EqualTo(dto.StartTime), "StartTime should match");
            Assert.That(created.EndTime, Is.EqualTo(dto.EndTime), "EndTime should match");
            Assert.That(created.TeacherId, Is.EqualTo(dto.TeacherId), "TeacherId should match");
            Assert.That(created.SubjectId, Is.EqualTo(dto.SubjectId), "SubjectId should match");
            Assert.That(created.Class.Id, Is.EqualTo(dto.Class.Id), "Class should match");
        });
    }

    [Test]
    public void ShouldThrowException_WhenDtoIsNull()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await this._curriculumService.CreateCurriculumAsync(null!),
            "Expected exception when dto is null");
    }
}

[TestFixture]
public class UpdateCurriculumAsyncTests : CurriculumServiceTestBase
{
    [Test]
    public async Task ShouldUpdateCurriculum_WhenDtoIsValid()
    {
        // Arrange
        var original = this.testDb.Curriculum1;

        var dto = new CurriculumDto
        {
            Id = original.Id,
            DayOfWeek = "Tuesday",
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(10, 45, 0),
            TeacherId = this.testDb.Teacher2.Id,
            SubjectId = this.testDb.Subject2.Id,
            Class = original.Class,
            Teacher = this.testDb.Teacher2,
            Subject = this.testDb.Subject2
        };

        // Act
        await this._curriculumService.UpdateCurriculumAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Curriculum>(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Curriculum should exist");
            Assert.That(updated!.DayOfWeek, Is.EqualTo("Tuesday"));
            Assert.That(updated.StartTime, Is.EqualTo(new TimeSpan(10, 0, 0)));
            Assert.That(updated.EndTime, Is.EqualTo(new TimeSpan(10, 45, 0)));
            Assert.That(updated.TeacherId, Is.EqualTo(this.testDb.Teacher2.Id));
            Assert.That(updated.SubjectId, Is.EqualTo(this.testDb.Subject2.Id));
        });
    }

    [Test]
    public void ShouldThrowException_WhenCurriculumDoesNotExist()
    {
        // Arrange
        var dto = new CurriculumDto
        {
            Id = Guid.NewGuid(),
            DayOfWeek = "Monday",
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(8, 45, 0),
            TeacherId = this.testDb.Teacher1.Id,
            SubjectId = this.testDb.Subject1.Id,
            Class = this.testDb.Class1,
            Teacher = this.testDb.Teacher1,
            Subject = this.testDb.Subject1
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._curriculumService.UpdateCurriculumAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Curriculum not found."));
    }
}

[TestFixture]
public class DeleteCurriculumAsyncTests : CurriculumServiceTestBase
{
    [Test]
    public async Task ShouldDeleteCurriculum_WhenValidIdProvided()
    {
        // Arrange
        var curriculumId = this.testDb.Curriculum2.Id;

        // Act
        await this._curriculumService.DeleteCurriculumAsync(curriculumId);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Curriculum>(curriculumId);
        Assert.That(deleted, Is.Null, "Curriculum should be deleted from the database");
    }

    [Test]
    public void ShouldThrowException_WhenIdDoesNotExist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._curriculumService.DeleteCurriculumAsync(invalidId));

        Assert.That(ex!.Message, Is.EqualTo($"Entity of type Curriculum with id {invalidId} could not be found"));
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._curriculumService.DeleteCurriculumAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Provided curriculum ID is empty"));
    }
}





