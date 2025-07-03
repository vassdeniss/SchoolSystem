using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class PrincipalServiceTestBase : UnitTestBase
{
    protected PrincipalService _principalService;

    [SetUp]
    public void SetupService()
    {
        this._principalService = new PrincipalService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetAllPrincipalsTests : PrincipalServiceTestBase
{
    [Test]
    public async Task ShouldReturnAllExistingPrincipals_WithCorrectIdsAndSpecializations()
    {
        // Arrange
        var expectedPrincipals = new[] { this.testDb.Principal1, this.testDb.Principal2, this.testDb.Principal3 };
        var expectedIds = expectedPrincipals.Select(p => p.Id).ToArray();
        var expectedSpecs = expectedPrincipals.Select(p => p.Specialization).ToArray();

        // Act
        var result = await this._principalService.GetAllPrincipalsAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Select(p => p.Id), Is.EquivalentTo(expectedIds), "Returned IDs should match expected");
            Assert.That(result.Select(p => p.Specialization), Is.EquivalentTo(expectedSpecs), "Returned specializations should match expected");
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenNoPrincipalsExist()
    {
        // Arrange
        this.testDb.ClearDatabase();

        // Act
        var result = await this._principalService.GetAllPrincipalsAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Expected non-null result even when no principals exist");
            Assert.That(result, Is.Empty, "Expected empty list when no principals are present in database");
        });
    }
}

[TestFixture]
public class GetPrincipalByIdTests : PrincipalServiceTestBase
{
    [Test]
    public async Task ShouldReturnCorrectPrincipalDto_WhenPrincipalExists()
    {
        // Arrange
        var expectedPrincipal = this.testDb.Principal1;
        var expectedUser = this.testDb.User1;

        // Act
        var result = await this._principalService.GetPrincipalByIdAsync(expectedPrincipal.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Returned PrincipalDto should not be null");
            Assert.That(result!.Id, Is.EqualTo(expectedPrincipal.Id), "Principal ID should match");
            Assert.That(result.UserId, Is.EqualTo(expectedUser.Id), "User ID should match");
            Assert.That(result.Specialization, Is.EqualTo(expectedPrincipal.Specialization), "Specialization should match");
            Assert.That(result.PhoneNumber, Is.EqualTo(expectedPrincipal.PhoneNumber), "Phone number should match");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenPrincipalIdIsEmpty()
    {
        // Arrange
        Guid emptyId = Guid.Empty;

        // Act
        var result = await this._principalService.GetPrincipalByIdAsync(emptyId);

        // Assert
        Assert.That(result, Is.Null, "Expected null result when passing Guid.Empty as Principal ID");
    }

    [Test]
    public async Task ShouldReturnNull_WhenPrincipalWithGivenIdDoesNotExist()
    {
        // Arrange
        Guid nonExistentId = Guid.NewGuid();

        // Act
        var result = await this._principalService.GetPrincipalByIdAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null, "Expected null when Principal with given ID does not exist");
    }
}

[TestFixture]
public class CreatePrincipalTests : PrincipalServiceTestBase
{
    [Test]
    public async Task ShouldCreatePrincipal_WhenValidDto()
    {
        // Arrange
        var dto = new PrincipalDto
        {
            UserId = this.testDb.User4.Id,
            Specialization = "Math",
            PhoneNumber = "088727202"
        };

        int countBefore = await this.repo.AllReadonly<Principal>().CountAsync();

        // Act
        await this._principalService.CreatePrincipalAsync(dto);

        // Assert
        var created = await this.repo.AllReadonly<Principal>()
            .FirstOrDefaultAsync(p => p.UserId == dto.UserId);

        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null, "Principal should be created and present in the database");
            Assert.That(created!.Specialization, Is.EqualTo(dto.Specialization), "Specialization should match");
            Assert.That(created.PhoneNumber, Is.EqualTo(dto.PhoneNumber), "Phone number should match");
        });

        int countAfter = await this.repo.AllReadonly<Principal>().CountAsync();
        Assert.That(countAfter, Is.EqualTo(countBefore + 1), "Principal count should increment after creation");
    }

    [Test]
    [Category("Validation")]
    public void ShouldThrowException_WhenUserIdIsEmpty()
    {
        // Arrange
        PrincipalDto dto = new()
        {
            UserId = Guid.Empty,
            Specialization = "Art",
            PhoneNumber = "0899224111"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._principalService.CreatePrincipalAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("UserId cannot be empty"));
    }

    [Test]
    public void ShouldThrowException_WhenPhoneNumberIsAlreadyUsed()
    {
        // Arrange
        var duplicatePhone = this.testDb.Principal1.PhoneNumber;

        var dto = new PrincipalDto
        {
            UserId = this.testDb.User4.Id,
            Specialization = "Informatics",
            PhoneNumber = duplicatePhone
        };

        // Act & Assert
        Assert.That(async () => await this._principalService.CreatePrincipalAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>()
                  .With.Message.EqualTo("Phone number already exists"));
    }

    [Test]
    public void ShouldThrowException_WhenUserAlreadyPrincipal()
    {
        // Arrange
        PrincipalDto dto = new() { UserId = this.testDb.User1.Id, Specialization = "Math", PhoneNumber = "12345" };

        // Act & Assert
        Assert.That(
            async () => await this._principalService.CreatePrincipalAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
}

[TestFixture]
public class UpdatePrincipalTests : PrincipalServiceTestBase
{

    [Test]
    public async Task ShouldUpdatePrincipal_WhenPrincipalExists()
    {
        // Arrange
        var principalId = this.testDb.Principal1.Id;
        var dto = new PrincipalDto
        {
            Id = principalId,
            Specialization = "Physics",
            PhoneNumber = "0899202514"
        };

        // Act
        await this._principalService.UpdatePrincipalAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Principal>(principalId);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Principal should exist after update");
            Assert.That(updated!.Specialization, Is.EqualTo(dto.Specialization), "Specialization should be updated");
            Assert.That(updated.PhoneNumber, Is.EqualTo(dto.PhoneNumber), "Phone number should be updated");
        });
    }

    [Test]
    public async Task ShouldAllowUpdate_WhenPhoneNumberIsUnchanged_ForSamePrincipal()
    {
        // Arrange
        var principalId = testDb.Principal1.Id;
        var originalPhone = testDb.Principal1.PhoneNumber;
        var dto = new PrincipalDto { Id = principalId, Specialization = "Updated Spec", PhoneNumber = originalPhone };

        // Act
        await _principalService.UpdatePrincipalAsync(dto);

        // Assert
        var updated = await repo.GetByIdAsync<Principal>(principalId);
        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Principal must still exist");
            Assert.That(updated!.PhoneNumber, Is.EqualTo(originalPhone), "Phone remains unchanged");
            Assert.That(updated.Specialization, Is.EqualTo(dto.Specialization), "Specialization is updated");
        });
    }

    [Test]
    public void ShouldThrowException_WhenTryingToUpdateWithExistingPhoneNumber()
    {
        // Arrange
        string existingPhone = testDb.Principal1.PhoneNumber;
        var dto = new PrincipalDto { Id = testDb.Principal2.Id, Specialization = "Bio", PhoneNumber = existingPhone };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _principalService.UpdatePrincipalAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("A principal with this phone number already exists."));
    }

    [Test]
    public void ShouldNotUpdatePrincipal_WhenPrincipalNotFound()
    {
        // Arrange
        PrincipalDto dto = new() { Id = Guid.NewGuid(), Specialization = "Physics", PhoneNumber = "67890" };

        // Act & Assert
        Assert.That(
            async () => await this._principalService.UpdatePrincipalAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
}

[TestFixture]
public class DeletePrincipalTests : PrincipalServiceTestBase
{

    [Test]
    public async Task ShouldDeletePrincipal_WhenPrincipalExists()
    {
        // Arrange
        Guid id = this.testDb.Principal3.Id;

        // Act
        await this._principalService.DeletePrincipalAsync(id);

        // Assert
        var deleted = await this.repo.GetByIdAsync<Principal>(id);
        Assert.That(deleted, Is.Null, "Principal should no longer exist in database after deletion");
    }

    [Test]
    public void ShouldThrowException_WhenPrincipalIsAssignedToSchool()
    {
        // Arrange
        var principalId = this.testDb.Principal1.Id;

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._principalService.DeletePrincipalAsync(principalId));

        Assert.That(ex!.Message, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void ShouldThrowException_WhenPrincipalDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._principalService.DeletePrincipalAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("Principal not found."));
    }
}

