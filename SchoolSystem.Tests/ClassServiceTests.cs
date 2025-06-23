using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

[TestFixture]
public class ClassServiceTests : UnitTestBase
{
    private ClassService _classService;
    
    [SetUp]
    public void SetUp()
    {
        this._classService = new ClassService(this.repo, this.mapper);
    }

    [Test]
    public async Task GetClassesBySchoolIdAsync_ShouldReturnClassesForGivenSchool()
    {
        // Arrange
        Guid schoolId = this.testDb.School1.Id;

        // Act
        IEnumerable<ClassDto> result = await this._classService.GetClassesBySchoolIdAsync(schoolId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.All(c => c.SchoolId == schoolId));
        Assert.That(result, Is.Ordered.Descending.By("Year"));
    }

    [Test]
    public async Task GetClassByIdAsync_ShouldReturnClass_WhenClassExists()
    {
        // Arrange
        Guid classId = this.testDb.Class1.Id;

        // Act
        ClassDto? result = await this._classService.GetClassByIdAsync(classId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(classId));
        Assert.That(result.Name, Is.EqualTo(this.testDb.Class1.Name));
    }

    [Test]
    public async Task GetClassByIdAsync_ShouldReturnNull_WhenClassNotFound()
    {
        // Arrange
        
        // Act
        ClassDto? result = await this._classService.GetClassByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateClassAsync_ShouldAddNewClass()
    {
        // Arrange
        ClassDto dto = new()
        {
            Name = "New Class",
            Year = 2023,
            Term = "Fall",
            SchoolId = this.testDb.School1.Id
        };
        int countBefore = await repo.AllReadonly<Class>().CountAsync();

        // Act
        await this._classService.CreateClassAsync(dto);

        // Assert
        int countAfter = await repo.AllReadonly<Class>().CountAsync();
        Assert.That(countAfter, Is.EqualTo(countBefore + 1));

        Class? createdClass = await repo.AllReadonly<Class>()
            .Where(c => c.Name == "New Class" && c.SchoolId == dto.SchoolId)
            .FirstOrDefaultAsync();
        Assert.That(createdClass, Is.Not.Null);
        Assert.That(createdClass.Year, Is.EqualTo(2023));
        Assert.That(createdClass.Term, Is.EqualTo("Fall"));
    }

    [Test]
    public async Task UpdateClassAsync_ShouldUpdateExistingClass()
    {
        // Arrange
        ClassDto dto = new()
        {
            Id = this.testDb.Class1.Id,
            Name = "Updated Class Name",
            Year = 2030,
            Term = "Spring",
            SchoolId = this.testDb.School1.Id
        };

        // Act
        await this._classService.UpdateClassAsync(dto);

        // Assert
        Class updatedClass = await repo.GetByIdAsync<Class>(dto.Id);
        Assert.That(updatedClass, Is.Not.Null);
        Assert.That(updatedClass.Name, Is.EqualTo("Updated Class Name"));
        Assert.That(updatedClass.Year, Is.EqualTo(2030));
        Assert.That(updatedClass.Term, Is.EqualTo("Spring"));
    }

    [Test]
    public void UpdateClassAsync_ShouldThrowException_WhenClassNotFound()
    {
        // Arrange
        ClassDto dto = new()
        {
            Id = Guid.NewGuid(),
            Name = "Nonexistent Class",
            Year = 2025,
            Term = "Spring",
            SchoolId = this.testDb.School1.Id
        };

        // Act & Assert
        Assert.That(async () => await this._classService.UpdateClassAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("Class not found."));
    }

    [Test]
    public async Task DeleteClassAsync_ShouldDeleteClass_WhenClassExists()
    {
        // Arrange
        Guid classId = this.testDb.Class2.Id;

        // Act
        await this._classService.DeleteClassAsync(classId);

        // Assert
        Class deletedClass = await repo.GetByIdAsync<Class>(classId);
        Assert.That(deletedClass, Is.Null);
    }
}
