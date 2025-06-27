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
public class GetPrincipalByIdTests : PrincipalServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldReturnPrincipalDto_WhenPrincipalExists()
    {
        // Arrange
        Guid id = this.testDb.Principal1.Id;
        Guid userId = this.testDb.User1.Id;

        // Act
        PrincipalDto? result = await this._principalService.GetPrincipalByIdAsync(id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.UserId, Is.EqualTo(userId));
        });
    }

    [Test]
    [Category("EdgeCase")]
    public async Task ShouldReturnNull_WhenIdIsEmpty()
    {
        // Arrange
        Guid emptyId = Guid.Empty;

        // Act
        PrincipalDto? result = await this._principalService.GetPrincipalByIdAsync(emptyId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("InvalidInput")]
    public async Task ShouldReturnNull_WhenPrincipalNotFound()
    {
        // Arrange & Act
        PrincipalDto? result = await this._principalService.GetPrincipalByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [TestCase("principal1")]
    [TestCase("principal2")]
    [Category("ParameterizedTest")]
    public async Task ShouldReturnCorrectUserId(string principalAlias)
    {
        var (principalId, expectedUserId) = principalAlias switch
        {
            "principal1" => (this.testDb.Principal1.Id, this.testDb.User1.Id),
            "principal2" => (this.testDb.Principal2.Id, this.testDb.User2.Id),
            _ => throw new ArgumentException("Unknown alias")
        };

        var result = await this._principalService.GetPrincipalByIdAsync(principalId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.UserId, Is.EqualTo(expectedUserId));
    }
}

[TestFixture]
public class GetAllPrincipalsTests : PrincipalServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldReturnAllExistingPrincipals()
    {
        // Arrange
        var expectedCount = 3;

        // Act
        var result = (await this._principalService.GetAllPrincipalsAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedCount));
        });
    }

    [Test]
    [Category("EdgeCase")]
    public async Task ShouldReturnEmptyList_WhenNoPrincipalsExist()
    {
        // Arrange
        this.testDb.ClearPrincipalsAndDown();

        // Act
        var result = await this._principalService.GetAllPrincipalsAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        });
    }

    [Test]
    [Category("ParameterizedTest")]
    public async Task ShouldIncludeExpectedPrincipals()
    {
        // Act
        var result = (await this._principalService.GetAllPrincipalsAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Any(p => p.UserId == this.testDb.User1.Id), Is.True, "Principal1 should be present");
            Assert.That(result.Any(p => p.UserId == this.testDb.User2.Id), Is.True, "Principal2 should be present");
            Assert.That(result.Any(p => p.UserId == this.testDb.User3.Id), Is.True, "Principal3 should be present");
        });
    }
}

[TestFixture]
public class CreatePrincipalTests : PrincipalServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldCreatePrincipal_WhenValidDto()
    {
        // Arrange
        PrincipalDto dto = new() { UserId = this.testDb.User4.Id, Specialization = "Math", PhoneNumber = "12345" };
        int rankPageCountBefore = await this.repo.AllReadonly<Principal>()
            .CountAsync();

        // Act
        await this._principalService.CreatePrincipalAsync(dto);

        // Assert
        int principalCountAfter = await this.repo.AllReadonly<Principal>()
            .CountAsync();
        Assert.That(principalCountAfter, Is.EqualTo(rankPageCountBefore + 1));

        Principal? newPrincipalInDb = await this.repo.AllReadonly<Principal>()
            .Where(p => p.UserId == this.testDb.User4.Id)
            .FirstOrDefaultAsync();
        Assert.That(newPrincipalInDb, Is.Not.Null);
        Assert.That(newPrincipalInDb!.Specialization, Is.EqualTo("Math"));
        Assert.That(newPrincipalInDb.PhoneNumber, Is.EqualTo("12345"));
    }

    [Test]
    [Category("EdgeCase")]
    public async Task ShouldNotAllowCreation_WhenUserIdIsEmpty_ButItDoes()
    {
        // Arrange
        PrincipalDto dto = new()
        {
            UserId = Guid.Empty,
            Specialization = "Art",
            PhoneNumber = "0899224111"
        };

        // Act
        await this._principalService.CreatePrincipalAsync(dto);

        // Assert
        Principal? created = await this.repo.AllReadonly<Principal>()
            .Where(p => p.UserId == Guid.Empty)
            .FirstOrDefaultAsync();

        Assert.That(created, Is.Null, "Principal with empty UserId should not be created.");
    }

    [Test]
    [Category("InvalidInput")]
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
    [Category("InvalidInput")]
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
    [Category("HappyPath")]
    public async Task ShouldUpdatePrincipal_WhenPrincipalExists()
    {
        // Arrange
        PrincipalDto dto = new() { Id = this.testDb.Principal1.Id, Specialization = "Physics", PhoneNumber = "67890" };

        // Act
        await this._principalService.UpdatePrincipalAsync(dto);

        // Assert
        Assert.That(this.testDb.Principal1.Specialization, Is.EqualTo("Physics"));
        Assert.That(this.testDb.Principal1.PhoneNumber, Is.EqualTo("67890"));
    }

    [Test]
    [Category("EdgeCase")]
    public void ShouldThrowException_WhenPhoneNumberAlreadyExists()
    {
        // Arrange
        string duplicatePhone = this.testDb.Principal1.PhoneNumber;

        PrincipalDto dto = new()
        {
            Id = this.testDb.Principal2.Id,
            Specialization = "Biology",
            PhoneNumber = duplicatePhone
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._principalService.UpdatePrincipalAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("A principal with this phone number already exists."));
    }

    [Test]
    [Category("InvalidInput")]
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
    [Category("HappyPath")]
    public async Task ShouldDeletePrincipal_WhenPrincipalExists()
    {
        Guid id = this.testDb.Principal3.Id;

        await this._principalService.DeletePrincipalAsync(id);

        Assert.That(await this.repo.GetByIdAsync<Principal>(id), Is.Null);
    }

    [Test]
    [Category("EdgeCase")]
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
    [Category("InvalidInput")]
    public void ShouldThrowException_WhenPrincipalDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid(); // ID, който със сигурност не съществува в базата

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._principalService.DeletePrincipalAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("Principal not found."));
    }
}

