using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class AttendanceServiceTestBase : UnitTestBase
{
    protected AttendanceService _attendanceService;

    [SetUp]
    public void SetupAttendanceService()
    {
        this._attendanceService = new AttendanceService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetAttendancesByStudentIdAsyncTests : AttendanceServiceTestBase
{
    [Test]
    public async Task ShouldReturnAttendances_WhenStudentHasRecords()
    {
        // Arrange
        var studentId = this.testDb.Student1.Id;

        // Act
        var result = await this._attendanceService.GetAttendancesByStudentIdAsync(studentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Count(), Is.EqualTo(2), "Expected two attendance records");
            Assert.That(result.All(a => a.StudentId == studentId), Is.True, "All records should belong to the same student");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenStudentHasNoAttendanceRecords()
    {
        // Arrange
        var studentWithoutRecords = this.testDb.Student2.Id;

        // Act
        var result = await this._attendanceService.GetAttendancesByStudentIdAsync(studentWithoutRecords);

        // Assert
        Assert.That(result, Is.Empty, "Expected no records for student with no attendances");
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenStudentIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await this._attendanceService.GetAttendancesByStudentIdAsync(invalidId);

        // Assert
        Assert.That(result, Is.Empty, "Expected no records for nonexistent student ID");
    }

    [Test]
    public void ShouldThrowException_WhenStudentIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._attendanceService.GetAttendancesByStudentIdAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Student ID cannot be empty"));
    }
}

[TestFixture]
public class GetAttendanceByIdAsyncTests : AttendanceServiceTestBase
{
    [Test]
    public async Task ShouldReturnAttendance_WhenValidIdProvided()
    {
        // Arrange
        var validId = this.testDb.Attendance1.Id;

        // Act
        var result = await this._attendanceService.GetAttendanceByIdAsync(validId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Attendance should be found");
            Assert.That(result!.Id, Is.EqualTo(validId), "ID should match");
            Assert.That(result.StudentId, Is.EqualTo(this.testDb.Student1.Id), "Student ID should match");
            Assert.That(result.SubjectId, Is.EqualTo(this.testDb.Subject1.Id), "Subject ID should match");
            Assert.That(result.AbsenceType, Is.EqualTo("Excused"), "AbsenceType should match");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenAttendanceIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await this._attendanceService.GetAttendanceByIdAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null, "Should return null for non-existent attendance ID");
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._attendanceService.GetAttendanceByIdAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Attendance ID cannot be empty"));
    }
}

[TestFixture]
public class CreateAttendanceAsyncTests : AttendanceServiceTestBase
{
    [Test]
    public async Task ShouldCreateAttendance_WhenDtoIsValid()
    {
        // Arrange
        var dto = new AttendanceDto
        {
            Id = Guid.NewGuid(),
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject1.Id,
            AbsenceType = "Excused",
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        // Act
        await this._attendanceService.CreateAttendanceAsync(dto);

        // Assert
        var created = await this.repo.GetByIdAsync<Attendance>(dto.Id);
        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null, "Attendance should be created");
            Assert.That(created!.StudentId, Is.EqualTo(dto.StudentId));
            Assert.That(created.SubjectId, Is.EqualTo(dto.SubjectId));
            Assert.That(created.AbsenceType, Is.EqualTo(dto.AbsenceType));
        });
    }

    [Test]
    public void ShouldThrowException_WhenStudentDoesNotExist()
    {
        var dto = new AttendanceDto
        {
            Id = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            SubjectId = this.testDb.Subject1.Id,
            AbsenceType = "Excused",
            Student = null!,
            Subject = this.testDb.Subject1
        };

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._attendanceService.CreateAttendanceAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Student not found."));
    }

    [Test]
    public void ShouldThrowException_WhenSubjectDoesNotExist()
    {
        var dto = new AttendanceDto
        {
            Id = Guid.NewGuid(),
            StudentId = this.testDb.Student1.Id,
            SubjectId = Guid.NewGuid(),
            AbsenceType = "Excused",
            Student = this.testDb.Student1,
            Subject = null!
        };

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._attendanceService.CreateAttendanceAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Subject not found."));
    }
}

[TestFixture]
public class UpdateAttendanceAsyncTests : AttendanceServiceTestBase
{
    [Test]
    public async Task ShouldUpdateAttendance_WhenDtoIsValid()
    {
        // Arrange
        var dto = new AttendanceDto
        {
            Id = this.testDb.Attendance1.Id,
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject2.Id,
            AbsenceType = "Unexcused",
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject2
        };

        // Act
        await this._attendanceService.UpdateAttendanceAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Attendance>(dto.Id);
        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.SubjectId, Is.EqualTo(this.testDb.Subject2.Id), "SubjectId should be updated");
            Assert.That(updated.AbsenceType, Is.EqualTo("Unexcused"), "AbsenceType should be updated");
        });
    }

    [Test]
    public void ShouldThrowException_WhenAttendanceDoesNotExist()
    {
        // Arrange
        var dto = new AttendanceDto
        {
            Id = Guid.NewGuid(),
            StudentId = this.testDb.Student1.Id,
            SubjectId = this.testDb.Subject1.Id,
            AbsenceType = "Excused",
            Student = this.testDb.Student1,
            Subject = this.testDb.Subject1
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._attendanceService.UpdateAttendanceAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Attendance not found."));
    }

    [Test]
    public void ShouldThrowException_WhenDtoIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await this._attendanceService.UpdateAttendanceAsync(null!));

        Assert.That(ex!.ParamName, Is.EqualTo("dto"));
    }
}

[TestFixture]
public class DeleteAttendanceAsyncTests : AttendanceServiceTestBase
{
    [Test]
    public async Task ShouldDeleteAttendance_WhenValidIdProvided()
    {
        // Arrange
        var attendanceId = this.testDb.Attendance2.Id;

        // Act
        await this._attendanceService.DeleteAttendanceAsync(attendanceId);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Attendance>(attendanceId);
        Assert.That(deleted, Is.Null, "Attendance should be deleted");
    }

    [Test]
    public void ShouldThrowException_WhenAttendanceDoesNotExist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._attendanceService.DeleteAttendanceAsync(invalidId));

        Assert.That(ex!.Message, Is.EqualTo($"Entity of type Attendance with id {invalidId} could not be found"));
    }

    [Test]
    public void ShouldThrowException_WhenIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._attendanceService.DeleteAttendanceAsync(Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Provided attendance ID is empty"));
    }
}




