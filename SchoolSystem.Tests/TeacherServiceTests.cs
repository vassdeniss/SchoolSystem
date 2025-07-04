using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class TeacherServiceTestBase : UnitTestBase
{
    protected TeacherService _teacherService;

    [SetUp]
    public void SetupTeacherService()
    {
        this._teacherService = new TeacherService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetTeachersBySchoolIdAsyncTests : TeacherServiceTestBase
{
    [Test]
    public async Task ShouldReturnTeachersForSchool()
    {
        // Arrange
        var schoolId = this.testDb.School1.Id;

        // Act
        var result = await this._teacherService.GetTeachersBySchoolIdAsync(schoolId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Count(), Is.EqualTo(1), "Should return exactly one teacher");

            var dto = result.First();
            Assert.That(dto.Id, Is.EqualTo(this.testDb.Teacher1.Id), "Teacher ID should match");
            Assert.That(dto.Specialization, Is.EqualTo(this.testDb.Teacher1.Specialization), "Specialization should match");
            Assert.That(dto.UserId, Is.EqualTo(this.testDb.User13.Id), "UserId should match the seeded user");
        });
    }

    [Test]
    public async Task ShouldReturnEmpty_WhenNoTeachersInSchool()
    {
        // Arrange
        var nonExistingSchoolId = Guid.NewGuid();

        // Act
        var result = await this._teacherService.GetTeachersBySchoolIdAsync(nonExistingSchoolId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null even if no teachers exist");
            Assert.That(result, Is.Empty, "Should return an empty collection when no teachers are found");
        });
    }
}

[TestFixture]
public class GetTeacherByIdAsyncTests : TeacherServiceTestBase
{
    [Test]
    public async Task ShouldReturnTeacher_WhenValidIdProvided()
    {
        var teacherId = this.testDb.Teacher1.Id;

        var result = await this._teacherService.GetTeacherByIdAsync(teacherId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Expected teacher to be found");
            Assert.That(result!.Id, Is.EqualTo(teacherId), "Teacher ID should match");
            Assert.That(result.Specialization, Is.EqualTo(this.testDb.Teacher1.Specialization), "Specialization should match");
            Assert.That(result.UserId, Is.EqualTo(this.testDb.User13.Id), "UserId should match");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenTeacherWithGivenIdDoesNotExist()
    {
        var result = await this._teacherService.GetTeacherByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Null, "Expected null when teacher does not exist");
    }

    [Test]
    public async Task ShouldReturnNull_WhenIdIsEmptyGuid()
    {
        var result = await this._teacherService.GetTeacherByIdAsync(Guid.Empty);

        Assert.That(result, Is.Null, "Expected null when empty Guid is passed");
    }
}

[TestFixture]
public class CanTeacherManageStudentTests : TeacherServiceTestBase
{
    [Test]
    public async Task ShouldReturnTrue_WhenTeacherManagesStudentClass()
    {
        // Arrange
        var teacherId = this.testDb.Teacher1.Id;
        var studentId = this.testDb.Student1.Id;

        // Act
        var result = await this._teacherService.CanTeacherManageStudent(teacherId, studentId);

        // Assert
        Assert.That(result, Is.True, "Expected true when teacher teaches the student's class");
    }

    [Test]
    public async Task ShouldReturnFalse_WhenTeacherDoesNotManageStudentClass()
    {
        // Arrange
        var teacherId = this.testDb.Teacher2.Id;
        var studentId = this.testDb.Student1.Id;

        // Act
        var result = await this._teacherService.CanTeacherManageStudent(teacherId, studentId);

        // Assert
        Assert.That(result, Is.False, "Expected false when teacher does not teach student's class");
    }

    [Test]
    public void ShouldThrowException_WhenTeacherIdDoesNotExist()
    {
        // Arrange
        var invalidTeacherId = Guid.NewGuid();
        var validStudentId = this.testDb.Student1.Id;

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._teacherService.CanTeacherManageStudent(invalidTeacherId, validStudentId),
            "Expected exception when teacher ID is invalid");
    }

    [Test]
    public void ShouldThrowException_WhenStudentIdDoesNotExist()
    {
        // Arrange
        var validTeacherId = this.testDb.Teacher1.Id;
        var invalidStudentId = Guid.NewGuid();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._teacherService.CanTeacherManageStudent(validTeacherId, invalidStudentId),
            "Expected exception when student ID is invalid");
    }

    [Test]
    public void ShouldThrowException_WhenIdsAreEmpty()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._teacherService.CanTeacherManageStudent(Guid.Empty, Guid.Empty),
            "Expected exception when both IDs are empty");
    }
}

[TestFixture]
public class CreateTeacherAsyncTests : TeacherServiceTestBase
{
    [Test]
    public async Task ShouldCreateNewTeacher_WhenUserHasNoTeacherProfile()
    {
        // Arrange
        var dto = new TeacherDto
        {
            Id = Guid.NewGuid(),
            Specialization = "History",
            SchoolId = this.testDb.School1.Id,
            UserId = this.testDb.User9.Id,
            User = this.testDb.User9,
            School = this.testDb.School1
        };

        int countBefore = await this.repo.AllReadonly<Teacher>().CountAsync();

        // Act
        await this._teacherService.CreateTeacherAsync(dto);

        // Assert
        var created = await this.repo.AllReadonly<Teacher>()
            .FirstOrDefaultAsync(t => t.UserId == dto.UserId);

        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null, "Teacher should be created");
            Assert.That(created!.Specialization, Is.EqualTo(dto.Specialization));
            Assert.That(created.Schools.Any(s => s.Id == dto.SchoolId), Is.True, "School should be linked to teacher");
        });

        int countAfter = await this.repo.AllReadonly<Teacher>().CountAsync();
        Assert.That(countAfter, Is.EqualTo(countBefore + 1), "Teacher count should increase");
    }

    [Test]
    public async Task ShouldAppendSchoolToExistingTeacher_WhenTeacherAlreadyExists()
    {
        // Arrange
        var dto = new TeacherDto
        {
            Id = this.testDb.Teacher1.Id,
            Specialization = this.testDb.Teacher1.Specialization,
            SchoolId = this.testDb.School2.Id,
            UserId = this.testDb.Teacher1.UserId,
            User = this.testDb.Teacher1.User,
            School = this.testDb.School2
        };

        int schoolCountBefore = this.testDb.Teacher1.Schools.Count;

        // Act
        await this._teacherService.CreateTeacherAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Teacher>(this.testDb.Teacher1.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated.Schools.Count, Is.EqualTo(schoolCountBefore + 1), "Teacher should have one more school");
            Assert.That(updated.Schools.Any(s => s.Id == this.testDb.School2.Id), Is.True, "New school should be linked");
        });
    }

    [Test]
    public void ShouldThrowException_WhenSchoolIdIsInvalid()
    {
        var dto = new TeacherDto
        {
            Id = Guid.NewGuid(),
            Specialization = "Geography",
            SchoolId = Guid.NewGuid(),
            UserId = this.testDb.User10.Id
        };

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._teacherService.CreateTeacherAsync(dto),
            "Should throw when school does not exist");
    }

    [TestFixture]
    public class UpdateTeacherAsyncTests : TeacherServiceTestBase
    {
        [Test]
        public async Task ShouldUpdateSpecialization_WhenTeacherExists()
        {
            // Arrange
            var teacherId = this.testDb.Teacher1.Id;
            var newSpecialization = "Physics";

            var dto = new TeacherDto
            {
                Id = teacherId,
                Specialization = newSpecialization,
                UserId = this.testDb.Teacher1.UserId,
                SchoolId = this.testDb.School1.Id,
                User = this.testDb.Teacher1.User,
                School = this.testDb.School1
            };

            // Act
            await this._teacherService.UpdateTeacherAsync(dto);

            // Assert
            var updated = await this.repo.GetByIdAsync<Teacher>(teacherId);
            Assert.Multiple(() =>
            {
                Assert.That(updated, Is.Not.Null, "Teacher should exist after update");
                Assert.That(updated!.Specialization, Is.EqualTo(newSpecialization), "Specialization should be updated");
            });
        }

        [Test]
        public void ShouldThrowException_WhenTeacherDoesNotExist()
        {
            // Arrange
            var dto = new TeacherDto
            {
                Id = Guid.NewGuid(),
                Specialization = "Biology",
                UserId = this.testDb.User10.Id,
                SchoolId = this.testDb.School1.Id
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await this._teacherService.UpdateTeacherAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("Teacher not found."));
        }
    }

}

[TestFixture]
public class DeleteTeacherAsyncTests : TeacherServiceTestBase
{
    [Test]
    public async Task ShouldDeleteTeacher_WhenOnlyOneSchoolIsAssigned()
    {
        // Arrange
        var teacherId = this.testDb.Teacher2.Id;
        var schoolId = this.testDb.School2.Id;

        int countBefore = await this.repo.AllReadonly<Teacher>().CountAsync();

        // Act
        await this._teacherService.DeleteTeacherAsync(teacherId, schoolId);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Teacher>(teacherId);
        int countAfter = await this.repo.AllReadonly<Teacher>().CountAsync();

        Assert.Multiple(() =>
        {
            Assert.That(deleted, Is.Null, "Teacher should be deleted when only one school exists");
            Assert.That(countAfter, Is.EqualTo(countBefore - 1), "Teacher count should decrease by one");
        });
    }

    [Test]
    public async Task ShouldRemoveSchoolAssociation_WhenMultipleSchoolsExist()
    {
        // Arrange
        var teacher = this.testDb.Teacher1;
        teacher.Schools.Add(this.testDb.School2);
        await this.repo.SaveChangesAsync();

        int schoolsBefore = teacher.Schools.Count;

        // Act
        await this._teacherService.DeleteTeacherAsync(teacher.Id, this.testDb.School2.Id);

        // Assert
        var updatedTeacher = await this.repo.GetByIdAsync<Teacher>(teacher.Id);
        Assert.Multiple(() =>
        {
            Assert.That(updatedTeacher, Is.Not.Null, "Teacher should still exist");
            Assert.That(updatedTeacher!.Schools.Count, Is.EqualTo(schoolsBefore - 1), "One school should be removed");
            Assert.That(updatedTeacher.Schools.Any(s => s.Id == this.testDb.School2.Id), Is.False, "Removed school should no longer be associated");
        });
    }

    [Test]
    public void ShouldThrowException_WhenTeacherIdIsInvalid()
    {
        var schoolId = this.testDb.School1.Id;
        var invalidTeacherId = Guid.NewGuid();

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._teacherService.DeleteTeacherAsync(invalidTeacherId, schoolId),
            "Expected exception when teacher ID does not exist");
    }

    [Test]
    public void ShouldThrowException_WhenSchoolIdIsInvalidForRemoval()
    {
        var teacherId = this.testDb.Teacher1.Id;
        var invalidSchoolId = Guid.NewGuid();

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._teacherService.DeleteTeacherAsync(teacherId, invalidSchoolId),
            "Expected exception when school ID does not exist");
    }

    [Test]
    public void ShouldThrowException_WhenIdsAreEmpty()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._teacherService.DeleteTeacherAsync(Guid.Empty, Guid.Empty),
            "Expected exception when IDs are empty");
    }
}



