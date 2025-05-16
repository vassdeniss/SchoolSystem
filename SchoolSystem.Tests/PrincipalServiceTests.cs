using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;

namespace SchoolSystem.Tests;

[TestFixture]
public class PrincipalServiceTests : UnitTestBase
{
    private PrincipalService _principalService;

    [SetUp]
    public void SetUp()
    {
        this._principalService = new PrincipalService(this.repo, this.mapper);
    }

    [Test]
    public async Task GetAllPrincipalsAsync_ShouldReturnListOfPrincipalDto()
    {
        // Arrange

        // Act
        IEnumerable<PrincipalDto> result = await this._principalService.GetAllPrincipalsAsync();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(result.First().Specialization, Is.EqualTo("Specialization1"));
    }
    
    [Test]
    public async Task GetPrincipalByIdAsync_ShouldReturnPrincipalDto_WhenPrincipalExists()
    {
        // Arrange
        Guid id = this.testDb.Principal1.Id;
        Guid userId = this.testDb.User1.Id;
    
        // Act
        PrincipalDto? result = await this._principalService.GetPrincipalByIdAsync(id);
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
    }
    
    [Test]
    public async Task GetPrincipalByIdAsync_ShouldReturnNull_WhenPrincipalNotFound()
    {
        // Arrange
    
        // Act
        PrincipalDto? result = await this._principalService.GetPrincipalByIdAsync(Guid.NewGuid());
    
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void CreatePrincipalAsync_ShouldThrowException_WhenUserAlreadyPrincipal()
    {
        // Arrange
        PrincipalDto dto = new() { UserId = this.testDb.User1.Id, Specialization = "Math", PhoneNumber = "12345" };
        
        // Act & Assert
        Assert.That(
            async() => await this._principalService.CreatePrincipalAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
    
    [Test]
    public async Task CreatePrincipalAsync_ShouldCreatePrincipal_WhenValidDto()
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
    public async Task UpdatePrincipalAsync_ShouldUpdatePrincipal_WhenPrincipalExists()
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
    public void UpdatePrincipalAsync_ShouldNotUpdatePrincipal_WhenPrincipalNotFound()
    {
        // Arrange
        PrincipalDto dto = new() { Id = Guid.NewGuid(), Specialization = "Physics", PhoneNumber = "67890" };
    
        // Act & Assert
        Assert.That(
            async() => await this._principalService.UpdatePrincipalAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
    
    [Test]
    public async Task DeletePrincipalAsync_ShouldDeletePrincipal_WhenPrincipalExists()
    {
        // Arrange
        Guid id = this.testDb.Principal3.Id;
    
        // Act
        await this._principalService.DeletePrincipalAsync(id);
    
        // Assert
        Assert.That(await this.repo.GetByIdAsync<Principal>(id), Is.Null);
    }
}
