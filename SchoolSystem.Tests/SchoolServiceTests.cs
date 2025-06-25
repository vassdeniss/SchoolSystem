using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class SchoolServiceTestBase : UnitTestBase
{
    protected SchoolService _schoolService;

    [SetUp]
    public void SetupSchoolService()
    {
        this._schoolService = new SchoolService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetSchoolsAsyncTests : SchoolServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task GetSchoolsAsync_ShouldReturnListOfSchoolDto()
    {
        // Act
        IEnumerable<SchoolDto> result = await this._schoolService.GetSchoolsAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().Name, Is.EqualTo(this.testDb.School1.Name));
    }

    [Test]
    [Category("EdgeCase")]
    public async Task GetSchoolsAsync_ShouldReturnEmptyList_WhenNoSchoolsExist()
    {
        // Arrange
        this.testDb.ClearSchoolsAndDown();

        // Act
        var result = (await this._schoolService.GetSchoolsAsync()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetSchoolsAsync_ShouldIncludeExpectedSchools()
    {
        // Act
        var result = (await this._schoolService.GetSchoolsAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Any(s => s.Name == this.testDb.School1.Name), Is.True, "School1 should be present");
            Assert.That(result.Any(s => s.Name == this.testDb.School2.Name), Is.True, "School2 should be present");
        });
    }
}

[TestFixture]
public class GetSchoolByIdAsyncTests : SchoolServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task GetSchoolByIdAsync_ShouldReturnSchoolDto_WhenSchoolExists()
    {
        // Arrange
        Guid id = this.testDb.School1.Id;

        // Act
        SchoolDto? result = await this._schoolService.GetSchoolByIdAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo(this.testDb.School1.Name));
    }

    [Test]
    [Category("EdgeCase")]
    public async Task GetSchoolByIdAsync_ShouldReturnNull_WhenIdIsEmpty()
    {
        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(Guid.Empty);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("InvalidInput")]
    public async Task GetSchoolByIdAsync_ShouldReturnNull_WhenSchoolNotFound()
    {
        // Arrange
        Guid fakeId = Guid.NewGuid();

        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(fakeId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [TestCase("school1")]
    [TestCase("school2")]
    [Category("ParameterizedTest")]
    public async Task GetSchoolByIdAsync_ShouldReturnCorrectSchoolData(string schoolAlias)
    {
        // Arrange
        Guid expectedId = schoolAlias switch
        {
            "school1" => this.testDb.School1.Id,
            "school2" => this.testDb.School2.Id,
            _ => throw new ArgumentException("Unknown alias")
        };

        string expectedName = schoolAlias switch
        {
            "school1" => this.testDb.School1.Name,
            "school2" => this.testDb.School2.Name,
            _ => throw new ArgumentException("Unknown alias")
        };

        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(expectedId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(expectedId));
        Assert.That(result.Name, Is.EqualTo(expectedName));
    }
}

[TestFixture]
public class CreateSchoolAsyncTests : SchoolServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task CreateSchoolAsync_ShouldCreateSchool_WhenValidDto()
    {
        // Arrange
        SchoolDto dto = new()
        {
            Name = "New School",
            Address = "newaddress",
            PrincipalId = this.testDb.Principal3.Id
        };

        int schoolCountBefore = await this.repo.AllReadonly<School>().CountAsync();

        // Act
        await this._schoolService.CreateSchoolAsync(dto);

        // Assert
        int schoolCountAfter = await this.repo.AllReadonly<School>().CountAsync();
        Assert.That(schoolCountAfter, Is.EqualTo(schoolCountBefore + 1));

        School? newSchoolInDb = await this.repo.AllReadonly<School>()
            .Where(s => s.Name == "New School")
            .FirstOrDefaultAsync();

        Assert.That(newSchoolInDb, Is.Not.Null);
    }

    [Test]
    [Category("InvalidInput")]
    public void CreateSchoolAsync_ShouldThrowException_WhenPrincipalAlreadyManagingASchool()
    {
        // Arrange
        SchoolDto dto = new()
        {
            Name = "Another School",
            Address = "somewhere",
            PrincipalId = this.testDb.Principal1.Id // Already managing School1
        };

        // Act & Assert
        Assert.That(
            async () => await this._schoolService.CreateSchoolAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }

    [Test]
    [Category("EdgeCase")]
    public async Task CreateSchoolAsync_ShouldNotAllowCreation_WhenPrincipalIdIsEmpty()
    {
        // Arrange
        SchoolDto dto = new()
        {
            Name = "Nameless School",
            Address = "nowhere",
            PrincipalId = Guid.Empty
        };

        // Act
        await this._schoolService.CreateSchoolAsync(dto);

        // Assert
        School? created = await this.repo.AllReadonly<School>()
            .Where(s => s.PrincipalId == Guid.Empty)
            .FirstOrDefaultAsync();

        Assert.That(created, Is.Null, "School with empty PrincipalId should not be created.");
    }
}

[TestFixture]
public class UpdateSchoolAsyncTests : SchoolServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task UpdateSchoolAsync_ShouldUpdateSchool_WhenSchoolExists()
    {
        // Arrange
        SchoolDto dto = new() { Id = this.testDb.School1.Id, Name = "Updated School Name" };

        // Act
        await this._schoolService.UpdateSchoolAsync(dto);

        // Assert
        Assert.That(this.testDb.School1.Name, Is.EqualTo("Updated School Name"));
    }

    [Test]
    [Category("InvalidInput")]
    public void UpdateSchoolAsync_ShouldNotUpdateSchool_WhenSchoolNotFound()
    {
        // Arrange
        SchoolDto dto = new() { Id = Guid.NewGuid(), Name = "Updated School Name", PrincipalId = this.testDb.Principal1.Id };

        // Act & Assert
        Assert.That(
            async () => await this._schoolService.UpdateSchoolAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }

    [Test]
    [Category("InvalidInput")]
    public void UpdateSchoolAsync_ShouldThrowException_WhenPrincipalIdIsEmpty()
    {
        // Arrange
        SchoolDto dto = new()
        {
            Id = this.testDb.School1.Id,
            Name = "Updated Name",
            Address = "New Address",
            PrincipalId = Guid.Empty
        };

        // Act & Assert
        Assert.That(
            async () => await this._schoolService.UpdateSchoolAsync(dto),
            Throws.TypeOf<ArgumentException>());
    }
}

[TestFixture]
public class DeleteSchoolAsyncTests : SchoolServiceTestBase
{           
    [Test]
    [Category("InvalidInput")]
    public async Task DeleteSchoolAsync_ShouldDeleteSchool_WhenSchoolExists()
    {
        // Arrange
        Guid id = this.testDb.School1.Id;
    
        // Act
        await this._schoolService.DeleteSchoolAsync(id);
    
        // Assert
        Assert.That(await this.repo.GetByIdAsync<School>(id), Is.Null);
    }

    [Test]
    [Category("InvalidInput")]
    public void DeleteSchoolAsync_ShouldThrow_WhenSchoolDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.DeleteSchoolAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("School not found."));
    }

    [Test]
    [Category("EdgeCase")]
    public void DeleteSchoolAsync_ShouldThrow_WhenSchoolHasClasses()
    {
        // Arrange
        var schoolId = this.testDb.School1.Id;

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.DeleteSchoolAsync(schoolId));

        Assert.That(ex!.Message, Is.Not.Null.And.Not.Empty,
            "Expected exception when deleting school with existing classes.");
    }
}


