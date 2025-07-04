using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class GradeServiceTestBase : UnitTestBase
{
    protected GradeService _gradeService;

    [SetUp]
    public void SetupGradeService()
    {
        this._gradeService = new GradeService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetGradesByStudentIdAsyncTests : GradeServiceTestBase
{
    [Test]
    public async Task ShouldReturnGradesInDescendingOrder_WhenGradesExist()
    {
        // Arrange
        var studentId = this.testDb.Student1.Id;

        // Act
        var result = await this._gradeService.GetGradesByStudentIdAsync(studentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Count(), Is.EqualTo(2), "Expected two grades for the student");

            var ordered = result.ToList();
            Assert.That(ordered[0].GradeDate >= ordered[1].GradeDate, Is.True, "Grades should be in descending order by date");
            Assert.That(ordered.All(g => g.StudentId == studentId), Is.True, "All grades should belong to the correct student");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenStudentHasNoGrades()
    {
        // Arrange
        var studentWithNoGrades = this.testDb.Student2.Id;

        // Act
        var result = await this._gradeService.GetGradesByStudentIdAsync(studentWithNoGrades);

        // Assert
        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result, Is.Empty, "Expected no grades for student with no records");
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenStudentIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await this._gradeService.GetGradesByStudentIdAsync(invalidId);

        // Assert
        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result, Is.Empty, "Expected no grades for invalid student ID");
    }

    [Test]
    public void ShouldThrowException_WhenStudentIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._gradeService.GetGradesByStudentIdAsync(emptyId));

        Assert.That(ex!.Message, Is.EqualTo("Student ID cannot be empty"));
    }
}

[TestFixture]
public class GetGradeByIdAsyncTests : GradeServiceTestBase
{
    [Test]
    public async Task ShouldReturnGrade_WhenValidIdIsProvided()
    {
        // Arrange
        var validId = this.testDb.Grade1.Id;

        // Act
        var result = await this._gradeService.GetGradeByIdAsync(validId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Grade should be found");
            Assert.That(result!.Id, Is.EqualTo(validId), "Grade ID should match");
            Assert.That(result.GradeValue, Is.EqualTo(this.testDb.Grade1.GradeValue), "Grade value should match");
            Assert.That(result.GradeDate.Date, Is.EqualTo(this.testDb.Grade1.GradeDate.Date), "Grade date should match");
            Assert.That(result.StudentId, Is.EqualTo(this.testDb.Student1.Id), "Student ID should match");
            Assert.That(result.SubjectId, Is.EqualTo(this.testDb.Subject1.Id), "Subject ID should match");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenGradeIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await this._gradeService.GetGradeByIdAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null, "Should return null when grade is not found");
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._gradeService.GetGradeByIdAsync(emptyId));

        Assert.That(ex!.Message, Is.EqualTo("Grade ID cannot be empty"));
    }
}

[TestFixture]
public class CreateGradeAsyncTests : GradeServiceTestBase
{
    [Test]
    public async Task ShouldCreateGrade_WhenDtoIsValid()
    {
        // Arrange
        var dto = new GradeDto
        {
            Id = Guid.NewGuid(),
            GradeValue = 5,
            GradeDate = DateTime.Today,
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject1.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        // Act
        await this._gradeService.CreateGradeAsync(dto);

        // Assert
        var created = await this.repo.GetByIdAsync<Grade>(dto.Id);
        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null, "Grade should be created");
            Assert.That(created!.GradeValue, Is.EqualTo(dto.GradeValue), "Grade value should match");
            Assert.That(created.GradeDate.Date, Is.EqualTo(dto.GradeDate.Date), "Grade date should match");
            Assert.That(created.StudentId, Is.EqualTo(dto.StudentId), "Student ID should match");
            Assert.That(created.SubjectId, Is.EqualTo(dto.SubjectId), "Subject ID should match");
        });
    }

    [Test]
    public async Task ShouldCreateMultipleGrades_WhenCalledWithDifferentData()
    {
        // Arrange
        var grade1 = new GradeDto
        {
            Id = Guid.NewGuid(),
            GradeValue = 6,
            GradeDate = DateTime.Today.AddDays(-1),
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject1.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        var grade2 = new GradeDto
        {
            Id = Guid.NewGuid(),
            GradeValue = 4,
            GradeDate = DateTime.Today,
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject2.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject2
        };

        // Act
        await this._gradeService.CreateGradeAsync(grade1);
        await this._gradeService.CreateGradeAsync(grade2);

        // Assert
        var all = await this.repo.AllReadonly<Grade>().Where(g => g.StudentId == grade1.StudentId).ToListAsync();
        Assert.That(all.Any(g => g.Id == grade1.Id), Is.True, "First grade should be created");
        Assert.That(all.Any(g => g.Id == grade2.Id), Is.True, "Second grade should be created");
    }

    [TestCase(1)]
    [TestCase(0)]
    [TestCase(-5)]
    [TestCase(7)]
    [TestCase(999)]
    public void ShouldThrowException_WhenGradeValueIsOutsideValidRange(int invalidValue)
    {
        var dto = new GradeDto
        {
            Id = Guid.NewGuid(),
            GradeValue = invalidValue,
            GradeDate = DateTime.Today,
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject1.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await this._gradeService.CreateGradeAsync(dto));

        Assert.That(ex!.Message, Does.Contain("Grade value must be between 2 and 6"));
    }
}

[TestFixture]
public class UpdateGradeAsyncTests : GradeServiceTestBase
{
    [Test]
    public async Task ShouldUpdateGrade_WhenDtoIsValid()
    {
        // Arrange
        var original = this.testDb.Grade1;

        var dto = new GradeDto
        {
            Id = original.Id,
            GradeValue = 5,
            GradeDate = DateTime.Today,
            SubjectId = this.testDb.Subject2.Id,
            StudentId = original.StudentId,
            Subject = this.testDb.Subject2,
            Student = original.Student
        };

        // Act
        await this._gradeService.UpdateGradeAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Grade>(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Grade should exist");
            Assert.That(updated!.GradeValue, Is.EqualTo(5), "GradeValue should be updated");
            Assert.That(updated.GradeDate.Date, Is.EqualTo(DateTime.Today.Date), "GradeDate should be updated");
            Assert.That(updated.SubjectId, Is.EqualTo(this.testDb.Subject2.Id), "Subject should be updated");
        });
    }

    [Test]
    public void ShouldThrowException_WhenGradeDoesNotExist()
    {
        // Arrange
        var dto = new GradeDto
        {
            Id = Guid.NewGuid(),
            GradeValue = 3,
            GradeDate = DateTime.Today,
            SubjectId = this.testDb.Subject1.Id,
            StudentId = this.testDb.Student1.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._gradeService.UpdateGradeAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Grade not found."));
    }

    [TestCase(1)]
    [TestCase(0)]
    [TestCase(-5)]
    [TestCase(7)]
    [TestCase(999)]
    public void ShouldThrowException_WhenUpdatingGradeWithInvalidValue(int invalidValue)
    {
        var dto = new GradeDto
        {
            Id = this.testDb.Grade1.Id,
            GradeValue = invalidValue,
            GradeDate = DateTime.Today,
            SubjectId = this.testDb.Subject1.Id,
            StudentId = this.testDb.Student1.Id,
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await this._gradeService.UpdateGradeAsync(dto));

        Assert.That(ex!.Message, Does.Contain("Grade value must be between 2 and 6"));
    }
}

[TestFixture]
public class DeleteGradeAsyncTests : GradeServiceTestBase
{
    [Test]
    public async Task ShouldDeleteGrade_WhenValidIdIsProvided()
    {
        // Arrange
        var gradeId = this.testDb.Grade2.Id;

        // Act
        await this._gradeService.DeleteGradeAsync(gradeId);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Grade>(gradeId);
        Assert.That(deleted, Is.Null, "Grade should be deleted from the database");
    }

    [Test]
    public void ShouldThrowException_WhenIdDoesNotExist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._gradeService.DeleteGradeAsync(invalidId));

        Assert.That(ex!.Message, Is.EqualTo($"Entity of type Grade with id {invalidId} could not be found"));
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._gradeService.DeleteGradeAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Provided grade ID is empty"));
    }
}



