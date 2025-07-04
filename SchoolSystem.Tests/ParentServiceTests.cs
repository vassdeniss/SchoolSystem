using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class ParentServiceTestBase : UnitTestBase
{
    protected ParentService _parentService;

    [SetUp]
    public void SetupService()
    {
        this._parentService = new ParentService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetAllParentsAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldReturnAllParentsWithStudentsAndClasses()
    {
        // Act
        var result = await this._parentService.GetAllParentsAsync();

        // Assert
        Assert.That(result, Is.Not.Null.And.Not.Empty, "Expected non-empty list of parents");

        var parent = result.FirstOrDefault(p => p.Id == this.testDb.Parent1.Id);

        Assert.Multiple(() =>
        {
            Assert.That(parent, Is.Not.Null, "Expected Parent1 to be present in result");
            Assert.That(parent!.Students, Is.Not.Null.And.Not.Empty, "Parent should have students");
            Assert.That(parent.Students.First().Class, Is.Not.Null, "Student's class should be included");
        });
    }

    [Test]
    public async Task ShouldReturnParentWithEmptyStudentsList_WhenParentHasNoChildren()
    {
        // Arrange
        var unlinkedParent = new Parent
        {
            Id = Guid.NewGuid(),
            UserId = this.testDb.User3.Id,
            PhoneNumber = "0888123456"
        };
        await this.repo.AddAsync(unlinkedParent);
        await this.repo.SaveChangesAsync();

        // Act
        var result = await this._parentService.GetAllParentsAsync();
        var parentDto = result.FirstOrDefault(p => p.Id == unlinkedParent.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(parentDto, Is.Not.Null);
            Assert.That(parentDto!.Students, Is.Empty, "Parent should have empty Students list");
        });
    }
}

[TestFixture]
public class GetParentByIdAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldReturnParent_WhenValidIdIsProvided()
    {
        // Act
        var result = await this._parentService.GetParentByIdAsync(this.testDb.Parent1.Id);

        // Assert
        Assert.That(result, Is.Not.Null, "Parent should be returned when ID exists");
        Assert.That(result!.Id, Is.EqualTo(this.testDb.Parent1.Id), "Returned parent should have correct ID");
    }

    [Test]
    public async Task ShouldReturnNull_WhenParentDoesNotExist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var result = await this._parentService.GetParentByIdAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null, "Expected null when parent with given ID does not exist");
    }

    [Test]
    public async Task ShouldReturnNull_WhenEmptyGuidIsProvided()
    {
        // Act
        var result = await this._parentService.GetParentByIdAsync(Guid.Empty);

        // Assert
        Assert.That(result, Is.Null, "Expected null when empty Guid is passed");
    }
}

[TestFixture]
public class AddStudentToParentAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldAddStudentToParent_WhenBothEntitiesExist()
    {
        // Arrange
        Guid parentId = this.testDb.Parent2.Id;
        Guid studentId = this.testDb.Student3.Id;

        // Act
        await this._parentService.AddStudentToParentAsync(parentId, studentId);

        // Assert
        Parent updatedParent = await this.repo.GetByIdAsync<Parent>(parentId);

        Assert.Multiple(() =>
        {
            Assert.That(updatedParent.Students, Is.Not.Null.And.Not.Empty, "Parent should have students assigned");
            Assert.That(updatedParent.Students.Any(s => s.Id == studentId), Is.True, "Student should be linked to parent");
        });
    }

    [Test]
    public void ShouldThrowException_WhenParentDoesNotExist()
    {
        // Arrange
        Guid invalidParentId = Guid.NewGuid();
        Guid validStudentId = this.testDb.Student2.Id;

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.AddStudentToParentAsync(invalidParentId, validStudentId));

        Assert.That(ex!.Message, Is.EqualTo("Parent not found."));
    }

    [Test]
    public void ShouldThrowException_WhenStudentDoesNotExist()
    {
        // Arrange
        Guid validParentId = this.testDb.Parent1.Id;
        Guid invalidStudentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.AddStudentToParentAsync(validParentId, invalidStudentId));

        Assert.That(ex!.Message, Is.EqualTo("Student not found."));
    }

    [Test]
    public async Task ShouldThrowException_WhenStudentAlreadyAssignedToParent()
    {
        // Arrange
        Guid parentId = this.testDb.Parent1.Id;
        Guid studentId = this.testDb.Student1.Id;

        // First assignment
        await this._parentService.AddStudentToParentAsync(parentId, studentId);

        // Act & Assert – повторно добавяне
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.AddStudentToParentAsync(parentId, studentId));

        Assert.That(ex!.Message, Is.EqualTo("Student already assigned to parent."));
    }

    [Test]
    public void ShouldThrowException_WhenEmptyGuidsAreProvided()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            this._parentService.AddStudentToParentAsync(Guid.Empty, Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Invalid parent or student ID."));
    }
}

[TestFixture]
public class RemoveStudentFromParentAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldRemoveStudentFromParent_WhenBothEntitiesExistAndLinked()
    {
        // Arrange
        Guid parentId = this.testDb.Parent1.Id;
        Guid studentId = this.testDb.Student1.Id;

        // Потвърди, че студентът вече е асоцииран с родителя
        Parent parentBefore = await this.repo.GetByIdAsync<Parent>(parentId);
        Assert.That(parentBefore.Students.Any(s => s.Id == studentId), Is.True, "Student should be initially assigned");

        // Act
        await this._parentService.RemoveStudentFromParentAsync(parentId, studentId);

        // Assert
        Parent parentAfter = await this.repo.GetByIdAsync<Parent>(parentId);
        Assert.That(parentAfter.Students.Any(s => s.Id == studentId), Is.False, "Student should be removed from parent");
    }

    [Test]
    public void ShouldThrowException_WhenParentDoesNotExist()
    {
        // Arrange
        Guid invalidParentId = Guid.NewGuid();
        Guid validStudentId = this.testDb.Student2.Id;

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.RemoveStudentFromParentAsync(invalidParentId, validStudentId));

        Assert.That(ex!.Message, Is.EqualTo("Parent not found."));
    }

    [Test]
    public void ShouldThrowException_WhenStudentDoesNotExist()
    {
        // Arrange
        Guid validParentId = this.testDb.Parent2.Id;
        Guid invalidStudentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.RemoveStudentFromParentAsync(validParentId, invalidStudentId));

        Assert.That(ex!.Message, Is.EqualTo("Student not found."));
    }

    [Test]
    public async Task ShouldNotFail_WhenStudentIsNotLinkedToParent()
    {
        // Arrange
        Guid parentId = this.testDb.Parent2.Id;
        Guid studentId = this.testDb.Student1.Id;

        // Act
        await this._parentService.RemoveStudentFromParentAsync(parentId, studentId);

        // Assert
        Parent parent = await this.repo.GetByIdAsync<Parent>(parentId);
        Assert.That(parent.Students.Any(s => s.Id == studentId), Is.False, "Student should not be present");
    }

    [Test]
    public void ShouldThrowException_WhenEmptyGuidsAreProvided()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            this._parentService.RemoveStudentFromParentAsync(Guid.Empty, Guid.Empty));

        Assert.That(ex!.Message, Is.EqualTo("Invalid parent or student ID."));
    }
}

[TestFixture]
public class CreateParentAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldCreateParent_WhenUserIsNotAlreadyAParent()
    {
        // Arrange
        var dto = new ParentDto
        {
            UserId = this.testDb.User3.Id,
            PhoneNumber = "0899555533"
        };

        // Act
        await this._parentService.CreateParentAsync(dto);

        // Assert
        var createdParent = await this.repo.AllReadonly<Parent>()
            .FirstOrDefaultAsync(p => p.UserId == dto.UserId);

        Assert.Multiple(() =>
        {
            Assert.That(createdParent, Is.Not.Null, "Parent should be created");
            Assert.That(createdParent!.PhoneNumber, Is.EqualTo(dto.PhoneNumber), "PhoneNumber should match");
        });
    }

    [Test]
    public void ShouldThrowException_WhenUserIsAlreadyParent()
    {
        // Arrange
        var dto = new ParentDto
        {
            UserId = this.testDb.Parent1.UserId,
            PhoneNumber = "0888999933"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.CreateParentAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("User is already a parent."));
    }

    [Test]
    public void ShouldThrowException_WhenPhoneNumberAlreadyExists()
    {
        // Arrange
        var dto = new ParentDto
        {
            UserId = this.testDb.User3.Id,
            PhoneNumber = "0897111111"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.CreateParentAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Phone number is already in use."));
    }
}

[TestFixture]
public class UpdateParentAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldUpdatePhoneNumber_WhenNewPhoneNumberIsUnique()
    {
        // Arrange
        var dto = new ParentDto
        {
            Id = this.testDb.Parent1.Id,
            UserId = this.testDb.Parent1.UserId,
            PhoneNumber = "0899000000"
        };

        // Act
        await this._parentService.UpdateParentAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Parent>(dto.Id);
        Assert.That(updated.PhoneNumber, Is.EqualTo(dto.PhoneNumber), "Phone number should be updated");
    }

    [Test]
    public void ShouldThrowException_WhenPhoneNumberBelongsToAnotherParent()
    {
        // Arrange:
        var dto = new ParentDto
        {
            Id = this.testDb.Parent1.Id,
            UserId = this.testDb.Parent1.UserId,
            PhoneNumber = this.testDb.Parent2.PhoneNumber
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.UpdateParentAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Phone number is already in use."));
    }

    [Test]
    public async Task ShouldNotThrow_WhenPhoneNumberIsSameAsCurrent()
    {
        // Arrange: телефонът не се променя
        var dto = new ParentDto
        {
            Id = this.testDb.Parent3.Id,
            UserId = this.testDb.Parent3.UserId,
            PhoneNumber = this.testDb.Parent3.PhoneNumber
        };

        // Act & Assert
        Assert.DoesNotThrowAsync(() => this._parentService.UpdateParentAsync(dto));

        var updated = await this.repo.GetByIdAsync<Parent>(dto.Id);
        Assert.That(updated.PhoneNumber, Is.EqualTo(dto.PhoneNumber), "Phone number should remain unchanged");
    }

    [Test]
    public void ShouldThrowException_WhenParentDoesNotExist()
    {
        // Arrange
        var dto = new ParentDto
        {
            Id = Guid.NewGuid(),
            UserId = this.testDb.User3.Id,
            PhoneNumber = "0899112233"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.UpdateParentAsync(dto));

        Assert.That(ex!.Message, Is.EqualTo("Parent not found."));
    }
}

[TestFixture]
public class DeleteParentAsyncTests : ParentServiceTestBase
{
    [Test]
    public async Task ShouldDeleteParent_WhenParentExists()
    {
        // Arrange
        Guid parentId = this.testDb.Parent6.Id;

        // Act
        await this._parentService.DeleteParentAsync(parentId);

        // Assert
        var parent = await this.repo.AllReadonly<Parent>()
            .FirstOrDefaultAsync(p => p.Id == parentId);

        Assert.That(parent, Is.Null, "Parent should be deleted from the database");
    }

    [Test]

    public void ShouldThrowException_WhenParentDoesNotExist()
    {
        // Arrange
        Guid nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._parentService.DeleteParentAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("Parent not found."));
    }
}







