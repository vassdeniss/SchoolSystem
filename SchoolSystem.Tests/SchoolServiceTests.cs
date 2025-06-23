using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

[TestFixture]
public class SchoolServiceTests : UnitTestBase
{
    private SchoolService _schoolService;

    [SetUp]
    public void SetUp()
    {
        this._schoolService = new SchoolService(this.repo, this.mapper);
    }

    [Test]
    public async Task GetSchoolsAsync_ShouldReturnListOfSchoolDto()
    {
        // Arrange

        // Act
        IEnumerable<SchoolDto> result = await this._schoolService.GetSchoolsAsync();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().Name, Is.EqualTo(this.testDb.School1.Name));
    }
    
    [Test]
    public async Task GetSchoolByIdAsync_ShouldReturnSchoolDto_WhenSchoolExists()
    {
        // Arrange
        Guid id = this.testDb.School1.Id;
    
        // Act
        SchoolDto? result = await this._schoolService.GetSchoolByIdAsync(id);
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(this.testDb.School1.Name));
    }
    
    [Test]
    public async Task GetSchoolByIdAsync_ShouldReturnNull_WhenSchoolNotFound()
    {
        // Arrange
    
        // Act
        SchoolDto? result = await this._schoolService.GetSchoolByIdAsync(Guid.NewGuid());
    
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void CreateSchoolAsync_ShouldThrowException_WhenPrincipalAlreadyManagingASchool()
    {
        // Arrange
        SchoolDto dto = new() { Name = "New School", Address = "newaddress", PrincipalId = this.testDb.Principal1.Id };
        
        // Act & Assert
        Assert.That(
            async() => await this._schoolService.CreateSchoolAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
    
    [Test]
    public async Task CreateSchoolAsync_ShouldCreateSchool_WhenValidDto()
    {
        // Arrange
        SchoolDto dto = new() { Name = "New School", Address = "newaddress", PrincipalId = this.testDb.Principal3.Id };
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
    public void UpdateSchoolAsync_ShouldNotUpdateSchool_WhenSchoolNotFound()
    {
        // Arrange
        SchoolDto dto = new() { Id = Guid.NewGuid(), Name = "Updated School Name", PrincipalId = this.testDb.Principal1.Id };
    
        // Act & Assert
        Assert.That(
            async() => await this._schoolService.UpdateSchoolAsync(dto),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
    
    [Test]
    public async Task DeleteSchoolAsync_ShouldDeleteSchool_WhenSchoolExists()
    {
        // Arrange
        Guid id = this.testDb.School1.Id;
    
        // Act
        await this._schoolService.DeleteSchoolAsync(id);
    
        // Assert
        Assert.That(await this.repo.GetByIdAsync<School>(id), Is.Null);
    }
}
