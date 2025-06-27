using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class ClassServiceTestBase : UnitTestBase
{
    protected ClassService _classService;

    [SetUp]
    public void SetUp()
    {
        this._classService = new ClassService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetClassesBySchoolIdAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldReturnClassesForGivenSchool()
    {
        // Arrange
        Guid schoolId = this.testDb.School1.Id;

        // Act
        var result = await this._classService.GetClassesBySchoolIdAsync(schoolId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.All(c => c.SchoolId == schoolId));
        Assert.That(result, Is.Ordered.Descending.By("Year"));
    }

    [Test]
    [Category("EdgeCase")]
    public async Task ShouldReturnEmptyList_WhenSchoolHasNoClasses()
    {
        Guid schoolId = this.testDb.School2.Id;
        var result = await this._classService.GetClassesBySchoolIdAsync(schoolId);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    [Category("InvalidInput")]
    public async Task ShouldReturnEmpty_WhenSchoolIdDoesNotExist()
    {
        var result = await this._classService.GetClassesBySchoolIdAsync(Guid.NewGuid());
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [TestCase("school1", ExpectedResult = 2)]
    [TestCase("school2", ExpectedResult = 0)]
    [Category("ParameterizedTest")]
    public async Task<int> ShouldReturnCorrectCount(string schoolAlias)
    {
        Guid schoolId = schoolAlias switch
        {
            "school1" => this.testDb.School1.Id,
            "school2" => this.testDb.School2.Id,
            _ => throw new ArgumentException("Unknown alias")
        };

        var result = await this._classService.GetClassesBySchoolIdAsync(schoolId);
        return result.Count();
    }
}

[TestFixture]
public class GetClassByIdAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldReturnClass_WhenClassExists()
    {
        // Arrange
        Guid classId = this.testDb.Class1.Id;

        // Act
        var result = await this._classService.GetClassByIdAsync(classId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(classId));
        Assert.That(result.Name, Is.EqualTo(this.testDb.Class1.Name));
    }

    [Test]
    [Category("InvalidInput")]
    public async Task ShouldReturnNull_WhenClassNotFound()
    {
        // Act
        var result = await this._classService.GetClassByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }
}

[TestFixture]
public class CreateClassAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldAddNewClass()
    {
        // Arrange
        var dto = new ClassDto
        {
            Name = "New Class",
            Year = 2023,
            Term = "Fall",
            SchoolId = this.testDb.School1.Id
        };

        int countBefore = await this.repo.AllReadonly<Class>().CountAsync();

        // Act
        await this._classService.CreateClassAsync(dto);

        // Assert
        int countAfter = await this.repo.AllReadonly<Class>().CountAsync();
        Assert.That(countAfter, Is.EqualTo(countBefore + 1));

        var created = await this.repo.AllReadonly<Class>()
            .FirstOrDefaultAsync(c => c.Name == "New Class" && c.SchoolId == dto.SchoolId);

        Assert.That(created, Is.Not.Null);
        Assert.That(created!.Year, Is.EqualTo(2023));
        Assert.That(created.Term, Is.EqualTo("Fall"));
    }
}

[TestFixture]
public class UpdateClassAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldUpdateExistingClass()
    {
        // Arrange
        var dto = new ClassDto
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
        var updated = await this.repo.GetByIdAsync<Class>(dto.Id);
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Name, Is.EqualTo("Updated Class Name"));
        Assert.That(updated.Year, Is.EqualTo(2030));
        Assert.That(updated.Term, Is.EqualTo("Spring"));
    }

    [Test]
    [Category("InvalidInput")]
    public void ShouldThrow_WhenClassNotFound()
    {
        // Arrange
        var dto = new ClassDto
        {
            Id = Guid.NewGuid(),
            Name = "Nonexistent Class",
            Year = 2025,
            Term = "Spring",
            SchoolId = this.testDb.School1.Id
        };

        // Act & Assert
        Assert.That(
            async () => await this._classService.UpdateClassAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("Class not found."));
    }
}

[TestFixture]
public class DeleteClassAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldDeleteClass_WhenClassExists()
    {
        // Arrange
        Guid classId = this.testDb.Class2.Id;

        // Act
        await this._classService.DeleteClassAsync(classId);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Class>(classId);
        Assert.That(deleted, Is.Null);
    }
}

