using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class StudentServiceTestBase : UnitTestBase
{
    protected StudentService _studentService;

    [SetUp]
    public void SetupService()
    {
        this._studentService = new StudentService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetStudentsByClassAsyncTests : StudentServiceTestBase
{
    [Test]
    public async Task ShouldReturnAllStudentsAssignedToClass()
    {
        // Arrange
        Guid classId = this.testDb.Class1.Id;
        var expectedStudentIds = new[] { this.testDb.Student1.Id, this.testDb.Student2.Id };

        // Act
        IEnumerable<StudentDto> result = await this._studentService.GetStudentsByClassAsync(classId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result must not be null");
            Assert.That(result.Select(s => s.Id), Is.EquivalentTo(expectedStudentIds), "Returned student IDs must match expected");
            Assert.That(result.All(s => s.ClassId == classId), "All students must belong to the specified class");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenNoStudentsExistInClass()
    {
        // Arrange
        this.testDb.ClearDatabase();
        var classId = Guid.NewGuid();

        // Act
        var result = await _studentService.GetStudentsByClassAsync(classId);

        // Assert
        Assert.That(result, Is.Empty, "Expected empty list when no students exist");
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenClassIdDoesNotExist()
    {
        // Arrange
        Guid nonExistingClassId = Guid.NewGuid();

        // Act
        var result = await this._studentService.GetStudentsByClassAsync(nonExistingClassId);

        // Assert
        Assert.That(result, Is.Empty, "Expected empty list when classId does not exist");
    }

    [Test]
    public async Task ShouldReturnOnlyStudentsFromTheSpecifiedClass()
    {
        // Arrange
        Guid targetClassId = this.testDb.Class2.Id;
        var expectedStudentIds = new[] { this.testDb.Student3.Id };

        // Act
        var result = await this._studentService.GetStudentsByClassAsync(targetClassId);

        // Assert
        Assert.That(result.Select(s => s.Id), Is.EquivalentTo(expectedStudentIds));
    }
}

[TestFixture]
public class GetStudentAsyncTests : StudentServiceTestBase
{

    [Test]
    public async Task ShouldReturnStudentDto_WhenStudentWithGivenIdExists()
    {
        // Arrange
        Guid existingStudentId = this.testDb.Student1.Id;

        // Act
        StudentDto? result = await this._studentService.GetStudentAsync(existingStudentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Expected non-null result when student exists");
            Assert.That(result!.Id, Is.EqualTo(existingStudentId), "Returned student ID must match requested ID");
            Assert.That(result.UserId, Is.EqualTo(this.testDb.Student1.UserId), "Returned UserId must match Student1");
            Assert.That(result.ClassId, Is.EqualTo(this.testDb.Student1.ClassId), "Returned ClassId must match Student1");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenStudentWithGivenIdDoesNotExist()
    {
        // Arrange
        Guid nonExistingId = Guid.NewGuid();

        // Act
        StudentDto? result = await this._studentService.GetStudentAsync(nonExistingId);

        // Assert
        Assert.That(result, Is.Null, "Expected null when student with given ID does not exist");
    }
}

[TestFixture]
public class GetStudentsNotAssignedToParentAsyncTests : StudentServiceTestBase
{
    [Test]
    public async Task ShouldReturnStudentsNotLinkedToParent()
    {
        // Arrange
        Guid parentId = this.testDb.Parent1.Id;
        var expectedStudentIds = new[] { this.testDb.Student2.Id, this.testDb.Student3.Id };

        // Act
        var result = await this._studentService.GetStudentsNotAssignedToParentAsync(parentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Select(s => s.Id), Is.EquivalentTo(expectedStudentIds), "Returned students must exclude linked ones");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenNoStudentsExist()
    {
        // Arrange
        this.testDb.ClearDatabase();
        Guid parentId = Guid.NewGuid();

        // Act
        var result = await this._studentService.GetStudentsNotAssignedToParentAsync(parentId);

        // Assert
        Assert.That(result, Is.Empty, "Expected empty list when no students exist");
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenParentIdDoesNotExist()
    {
        // Arrange
        Guid nonExistingParentId = Guid.NewGuid();

        // Act
        var result = await this._studentService.GetStudentsNotAssignedToParentAsync(nonExistingParentId);

        // Assert
        Assert.That(result.Select(s => s.Id), Is.EquivalentTo(new[] { this.testDb.Student1.Id, this.testDb.Student2.Id, this.testDb.Student3.Id }));
    }
}

[TestFixture]
public class CreateStudentAsyncTests : StudentServiceTestBase
{
    [Test]
    public async Task ShouldCreateStudent_WhenValid()
    {
        // Arrange
        var dto = new StudentDto
        {
            UserId = Guid.NewGuid(),
            ClassId = this.testDb.Class1.Id
        };

        int studentCountBefore = await this.repo.AllReadonly<Student>().CountAsync();

        // Act
        await this._studentService.CreateStudentAsync(dto);

        // Assert
        int studentCountAfter = await this.repo.AllReadonly<Student>().CountAsync();
        Student? newStudent = await this.repo.AllReadonly<Student>()
            .FirstOrDefaultAsync(s => s.UserId == dto.UserId);

        Assert.Multiple(() =>
        {
            Assert.That(studentCountAfter, Is.EqualTo(studentCountBefore + 1),
                "Student count should increase by 1");
            Assert.That(newStudent, Is.Not.Null,
                "New student should not be null");
            Assert.That(newStudent!.ClassId, Is.EqualTo(dto.ClassId),
                "ClassId of new student should match input");
            Assert.That(newStudent.UserId, Is.EqualTo(dto.UserId),
                "UserId of new student should match input");
        });
    }

    [Test]
    public void ShouldThrowException_WhenUserIsAlreadyStudent()
    {
        // Arrange
        StudentDto dto = new()
        {
            UserId = this.testDb.Student1.UserId,
            ClassId = this.testDb.Class1.Id
        };

        // Act & Assert
        Assert.That(
            async () => await this._studentService.CreateStudentAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("User is already a student."));
    }

    [Test]
    public async Task ShouldCreateStudent_WhenUserIdIsEmptyGuid()
    {
        // Arrange
        var dto = new StudentDto
        {
            UserId = Guid.Empty,
            ClassId = this.testDb.Class1.Id
        };

        // Act
        await this._studentService.CreateStudentAsync(dto);

        // Assert
        Student? student = await this.repo.AllReadonly<Student>()
            .FirstOrDefaultAsync(s => s.UserId == Guid.Empty);

        Assert.Multiple(() =>
        {
            Assert.That(student, Is.Not.Null, "Student with empty UserId should still be created");
            Assert.That(student!.UserId, Is.EqualTo(Guid.Empty), "UserId should be Guid.Empty as input");
            Assert.That(student.ClassId, Is.EqualTo(dto.ClassId), "ClassId should match input");
        });
    }
}

[TestFixture]
public class UpdateStudentAsyncTests : StudentServiceTestBase
{
    [Test]
    public async Task ShouldUpdateClassId_WhenStudentExists()
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
        Assert.That(updatedStudent!.ClassId, Is.EqualTo(dto.ClassId));
    }

    [Test]
    public void ShouldThrowException_WhenStudentNotFound()
    {
        // Arrange
        StudentDto dto = new()
        {
            Id = Guid.NewGuid(),
            ClassId = this.testDb.Class1.Id
        };

        // Act & Assert
        Assert.That(
            async () => await this._studentService.UpdateStudentAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("Student not found."));
    }
}

[TestFixture]
public class DeleteStudentAsyncTests : StudentServiceTestBase
{
    [Test]
    public async Task ShouldDeleteStudent_WhenStudentExists()
    {
        // Arrange
        Guid id = this.testDb.Student1.Id;

        // Act
        await this._studentService.DeleteStudentAsync(id);

        // Assert
        var deletedStudent = await this.repo.GetByIdAsync<Student>(id);
        Assert.That(deletedStudent, Is.Null, "Student should no longer exist in database after deletion");
    }

    [Test]
    public void ShouldThrowException_WhenStudentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._studentService.DeleteStudentAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("Student not found."));
    }
}
