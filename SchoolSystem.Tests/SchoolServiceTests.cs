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
    public async Task ShouldReturnAllSchools_WithMappedPrincipalAndUser()
    {
        // Arrange
        var expectedSchoolIds = new[] { this.testDb.School1.Id, this.testDb.School2.Id };
        var expectedPrincipalIds = new[] { this.testDb.School1.Principal.Id, this.testDb.School2.Principal.Id };

        // Act
        var result = (await this._schoolService.GetSchoolsAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Select(s => s.Id), Is.EquivalentTo(expectedSchoolIds), "Returned school IDs should match expected");

            Assert.That(result.All(s => s.Principal != null), Is.True, "Each school should have a non-null Principal");
            Assert.That(result.All(s => s.Principal.User != null), Is.True, "Each Principal should have an associated User");
            Assert.That(result.Select(s => s.Principal.Id), Is.EquivalentTo(expectedPrincipalIds), "Principal IDs should match expected");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenNoSchoolsExist()
    {
        // Arrange
        this.testDb.ClearDatabase();

        // Act
        var result = (await this._schoolService.GetSchoolsAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null when no schools exist");
            Assert.That(result, Is.Empty, "Result should be an empty list when no schools are present");
        });
    }
}

[TestFixture]
public class GetSchoolByIdAsyncTests : SchoolServiceTestBase
{
    [Test]
    public async Task ShouldReturnCorrectSchoolDto_WhenSchoolExists()
    {
        // Arrange
        Guid schoolId = this.testDb.School1.Id;
        var expected = this.testDb.School1;

        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(schoolId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Expected a non-null SchoolDto");
            Assert.That(result!.Id, Is.EqualTo(expected.Id), "School ID mismatch");
            Assert.That(result.Name, Is.EqualTo(expected.Name), "School Name mismatch");
            Assert.That(result.Principal, Is.Not.Null, "Expected School to have a Principal");
            Assert.That(result.Principal.Id, Is.EqualTo(expected.Principal.Id), "Principal ID mismatch");
            Assert.That(result.Principal.User, Is.Not.Null, "Expected Principal to have associated User");
            Assert.That(result.Principal.User.Email, Is.EqualTo(expected.Principal.User.Email), "Principal's User Email mismatch");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenSchoolIdIsEmpty()
    {
        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(Guid.Empty);

        // Assert
        Assert.That(result, Is.Null, "Expected null when passing Guid.Empty as school ID");
    }

    [Test]
    public async Task ShouldReturnNull_WhenSchoolWithGivenIdDoesNotExist()
    {
        // Arrange
        Guid nonexistentId = Guid.NewGuid();

        // Act
        var result = await this._schoolService.GetSchoolByIdAsync(nonexistentId);

        // Assert
        Assert.That(result, Is.Null, "Expected result to be null when attempting to retrieve a school with a non-existent ID.");
    }
}

[TestFixture]
public class CreateSchoolAsyncTests : SchoolServiceTestBase
{
    [Test]
    public async Task ShouldCreateSchool_WhenDtoIsValid()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Name = "New School",
            Address = "newaddress",
            PrincipalId = this.testDb.Principal3.Id
        };

        var schoolCountBefore = await this.repo.AllReadonly<School>().CountAsync();

        // Act
        await this._schoolService.CreateSchoolAsync(dto);

        // Assert
        var schoolsAfter = await this.repo.AllReadonly<School>().ToListAsync();
        var newSchool = schoolsAfter.FirstOrDefault(s => s.Name == "New School");

        Assert.Multiple(() =>
        {
            Assert.That(schoolsAfter.Count, Is.EqualTo(schoolCountBefore + 1), "School count should increase by 1");
            Assert.That(newSchool, Is.Not.Null, "New school should exist in database");
            Assert.That(newSchool!.Address, Is.EqualTo(dto.Address), "School address mismatch");
            Assert.That(newSchool.PrincipalId, Is.EqualTo(dto.PrincipalId), "Principal ID mismatch");
        });
    }

    [Test]
    public void ShouldThrowException_WhenPrincipalAlreadyManagingASchool()
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
    public void ShouldThrowException_WhenSchoolNameAlreadyExists()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Name = "school1",
            Address = "duplicate address",
            PrincipalId = this.testDb.Principal3.Id
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.CreateSchoolAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("A school with this name already exists."));
    }

    [Test]
    public void ShouldThrowException_WhenPrincipalDoesNotExist()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Name = "Phantom School",
            Address = "ghost street",
            PrincipalId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<DbUpdateException>(async () =>
            await this._schoolService.CreateSchoolAsync(dto));

        Assert.That(ex, Is.Not.Null, "Expected an exception when using a non-existent PrincipalId");
    }
}

[TestFixture]
public class UpdateSchoolAsyncTests : SchoolServiceTestBase
{
    [Test]
    public async Task ShouldUpdateSchool_WhenSchoolExists()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Id = this.testDb.School1.Id,
            Name = "Updated School Name",
            Address = "Updated Address",
            PrincipalId = this.testDb.Principal3.Id
        };

        // Act
        await this._schoolService.UpdateSchoolAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<School>(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Expected the school to exist after update");
            Assert.That(updated!.Name, Is.EqualTo(dto.Name), "Name should be updated");
            Assert.That(updated.Address, Is.EqualTo(dto.Address), "Address should be updated");
            Assert.That(updated.PrincipalId, Is.EqualTo(dto.PrincipalId), "Principal ID should be updated");
        });
    }

    [Test]
    public void ShouldNotUpdateSchool_WhenSchoolNotFound()
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
    public void ShouldThrowException_WhenUpdatingSchoolWithNonExistentPrincipal()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Id = this.testDb.School1.Id,
            Name = "Updated School",
            Address = "Updated Address",
            PrincipalId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.UpdateSchoolAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Principal not found."));
    }

    [Test]
    public void ShouldThrowException_WhenUpdatingSchoolWithExistingName()
    {
        // Arrange
        var dto = new SchoolDto
        {
            Id = this.testDb.School1.Id,
            Name = this.testDb.School2.Name,
            Address = "Updated Address",
            PrincipalId = this.testDb.Principal3.Id
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.UpdateSchoolAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("A school with this name already exists."));
    }
}

[TestFixture]
public class DeleteSchoolAsyncTests : SchoolServiceTestBase
{           
    [Test]
    public async Task ShouldDeleteSchool_WhenSchoolExists()
    {
        // Arrange
        Guid id = this.testDb.School1.Id;
    
        // Act
        await this._schoolService.DeleteSchoolAsync(id);
    
        // Assert
        Assert.That(await this.repo.GetByIdAsync<School>(id), Is.Null);
    }

    [Test]
    public void ShouldThrowException_WhenSchoolDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._schoolService.DeleteSchoolAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("School not found."));
    }

    [Test]
    public void ShouldThrowException_WhenSchoolHasClasses()
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


