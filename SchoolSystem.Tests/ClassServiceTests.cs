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
    public async Task ShouldReturnAllClassesForGivenSchool_WhenClassesExist()
    {
        // Arrange
        Guid schoolId = this.testDb.School1.Id;
        var expectedClassIds = new[] { this.testDb.Class1.Id, this.testDb.Class2.Id };

        // Act
        var result = (await this._classService.GetClassesBySchoolIdAsync(schoolId)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Count, Is.EqualTo(2), "Should return two classes");
            Assert.That(result.All(c => c.SchoolId == schoolId), Is.True, "All returned classes must belong to the given school");
            Assert.That(result.Select(c => c.Id), Is.EquivalentTo(expectedClassIds), "Returned class IDs should match expected");
            Assert.That(result, Is.Ordered.Descending.By("Year"), "Classes should be ordered by descending Year");
        });
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
    [Category("EdgeCase")]
    public async Task ShouldReturnEmptyList_WhenSchoolIdIsEmpty()
    {
        // Arrange
        Guid emptyGuid = Guid.Empty;

        // Act
        var result = await this._classService.GetClassesBySchoolIdAsync(emptyGuid);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null even if input Guid is empty");
            Assert.That(result, Is.Empty, "Expected empty result when SchoolId is Guid.Empty");
        });
    }

    [Test]
    [Category("InvalidInput")]
    public async Task ShouldReturnEmpty_WhenSchoolIdDoesNotExist()
    {
        var result = await this._classService.GetClassesBySchoolIdAsync(Guid.NewGuid());
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
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
        var expected = this.testDb.Class1;
        Guid classId = expected.Id;

        // Act
        var result = await this._classService.GetClassByIdAsync(classId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "Result should not be null for existing class ID");
            Assert.That(result!.Id, Is.EqualTo(expected.Id), "Class ID should match");
            Assert.That(result.Name, Is.EqualTo(expected.Name), "Class name should match");
            Assert.That(result.Term, Is.EqualTo(expected.Term), "Term should match");
            Assert.That(result.Year, Is.EqualTo(expected.Year), "Year should match");
            Assert.That(result.SchoolId, Is.EqualTo(expected.SchoolId), "SchoolId should match");
        });
    }

    [Test]
    [Category("EdgeCase")]
    public async Task ShouldReturnNull_WhenIdIsEmpty()
    {
        // Arrange
        Guid emptyId = Guid.Empty;

        // Act
        var result = await this._classService.GetClassByIdAsync(emptyId);

        // Assert
        Assert.That(result, Is.Null, "Expected null when class ID is Guid.Empty");
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
    public async Task ShouldAddNewClass_WhenValidDtoIsProvided()
    {
        // Arrange
        var dto = new ClassDto { Name = "New Class", Year = 2023, Term = "Fall", SchoolId = this.testDb.School1.Id };
        int countBefore = await this.repo.AllReadonly<Class>().CountAsync();

        // Act
        await this._classService.CreateClassAsync(dto);

        // Assert
        var allClasses = await this.repo.AllReadonly<Class>().ToListAsync();
        var created = allClasses.FirstOrDefault(c =>
            c.Name == dto.Name &&
            c.SchoolId == dto.SchoolId &&
            c.Year == dto.Year &&
            c.Term == dto.Term);

        Assert.Multiple(() =>
        {
            Assert.That(allClasses.Count, Is.EqualTo(countBefore + 1), "Total class count should increase by one");
            Assert.That(created, Is.Not.Null, "Created class should exist in the database");
            Assert.That(created!.Name, Is.EqualTo(dto.Name), "Class name should match");
            Assert.That(created.Year, Is.EqualTo(dto.Year), "Year should match");
            Assert.That(created.Term, Is.EqualTo(dto.Term), "Term should match");
            Assert.That(created.SchoolId, Is.EqualTo(dto.SchoolId), "SchoolId should match");
        });
    }

    [Test] //Waiting for term specification
    [Category("EdgeCase")]
    public void ShouldThrowException_WhenClassWithSameNameAndYearExistsInSchool()
    {
        // Arrange
        var dto = new ClassDto
        {
            Name = this.testDb.Class1.Name,
            Year = this.testDb.Class1.Year,
            Term = this.testDb.Class1.Term == "1" ? "2" : "1", // Change the term to show that it should not affect uniqueness

            SchoolId = this.testDb.Class1.SchoolId
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._classService.CreateClassAsync(dto));

        Assert.That(ex, Is.Not.Null, "Expected exception was not thrown");
        Assert.That(ex!.Message, Does.Contain("A class with the same name and starting year already exists in this school."));
    }


    [Test] 
    [Category("EdgeCase")]
    public void ShouldThrowException_WhenSchoolIdDoesNotExist()
    {
        // Arrange
        var dto = new ClassDto
        {
            Name = "Does not exist",
            Year = DateTime.Now.Year,
            Term = "Fall",
            SchoolId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._classService.CreateClassAsync(dto));

        Assert.That(ex!.Message, Does.Contain("SchoolId does not exist"));
    }

    [Test] //Waiting for term specification
    [Category("InvalidInput")]
    [TestCase("Summer")]
    [TestCase("Winter")]
    public void ShouldThrowException_WhenTermIsInvalid(string invalidTerm)
    {
        // Arrange
        var dto = new ClassDto
        {
            Name = "Invalid Term Class",
            Year = 2023,
            Term = invalidTerm,
            SchoolId = this.testDb.School1.Id
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await this._classService.CreateClassAsync(dto);
        });

        Assert.That(ex!.Message, Does.Contain("Term must be either 'Fall' or 'Spring'"));
    }

    [Test]
    [Category("InvalidInput")]
    [TestCase(-10)]
    [TestCase(0)]
    public void ShouldThrowException_WhenYearIsInvalid(int invalidYear)
    {
        // Arrange
        var dto = new ClassDto
        {
            Name = "Test Invalid Year",
            Year = invalidYear,
            Term = "Autumn",
            SchoolId = this.testDb.School1.Id
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._classService.CreateClassAsync(dto));

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message, Does.Contain("Year is not valid"));
    }
}

[TestFixture]
public class UpdateClassAsyncTests : ClassServiceTestBase
{
    [Test]
    [Category("HappyPath")]
    public async Task ShouldUpdateClass_WhenValidDtoIsProvided()
    {
        // Arrange
        var existing = this.testDb.Class1;
        var dto = new ClassDto
        {
            Id = existing.Id,
            Name = "Updated Class Name",
            Year = 2030,
            Term = "Spring",
            SchoolId = this.testDb.School1.Id
        };

        // Act
        await this._classService.UpdateClassAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Class>(dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null, "Updated class should still exist in the database");
            Assert.That(updated!.Name, Is.EqualTo(dto.Name), "Name should be updated");
            Assert.That(updated.Year, Is.EqualTo(dto.Year), "Year should be updated");
            Assert.That(updated.Term, Is.EqualTo(dto.Term), "Term should be updated");
            Assert.That(updated.SchoolId, Is.EqualTo(dto.SchoolId), "SchoolId should be updated");
        });
    }

    [Test]
    [Category("EdgeCase")]
    public void ShouldThrowArgumentException_WhenUpdatingClassWithNonExistentSchoolId()
    {
        // Arrange
        var existing = this.testDb.Class2;

        var dto = new ClassDto
        {
            Id = existing.Id,
            Name = "Class With Invalid School",
            Year = existing.Year,
            Term = existing.Term,
            SchoolId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._classService.UpdateClassAsync(dto));

        Assert.That(ex!.Message, Does.Contain("SchoolId does not exist."));
    }

    [Test] //Waiting for term specification
    [Category("EdgeCase")]
    public void ShouldThrowException_WhenUpdatingClassToDuplicateNameWithinSameSchool()
    {
        // Arrange
        var target = this.testDb.Class2;
        var duplicateName = this.testDb.Class1.Name;

        var dto = new ClassDto
        {
            Id = target.Id,
            Name = duplicateName,
            Year = target.Year,
            Term = target.Term,
            SchoolId = target.SchoolId
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._classService.UpdateClassAsync(dto));

        Assert.That(ex!.Message, Does.Contain("A class with the same name already exists in the school."));
    }

    [Test]
    [Category("InvalidInput")]
    [TestCase(0)]
    [TestCase(-100)]
    public void ShouldThrowException_WhenYearIsInvalid(int invalidYear)
    {
        // Arrange
        var existing = this.testDb.Class1;

        var dto = new ClassDto
        {
            Id = existing.Id,
            Name = "Invalid Year Class",
            Year = invalidYear,
            Term = existing.Term,
            SchoolId = existing.SchoolId
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._classService.UpdateClassAsync(dto));

        Assert.That(ex!.Message, Does.Contain("Year is not valid"));
    }

    [Test]
    [Category("InvalidInput")]
    public void ShouldThrowException_WhenClassNotFound()
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

    [Test] //Waiting for term specification
    [Category("InvalidInput")]
    [TestCase("Winter")]
    [TestCase("Summer")]
    public void ShouldThrowArgumentException_WhenTermIsInvalid(string invalidTerm)
    {
        // Arrange
        var existing = this.testDb.Class1;

        var dto = new ClassDto
        {
            Id = existing.Id,
            Name = "Class With Invalid Term",
            Year = existing.Year,
            Term = invalidTerm,
            SchoolId = existing.SchoolId
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await this._classService.UpdateClassAsync(dto));

        Assert.That(ex!.Message, Does.Contain("Term must be either 'Fall' or 'Spring'."));
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

    [Test]
    [Category("EdgeCase")]
    public void ShouldThrowException_WhenTryingToDeleteClassWithEnrolledStudents()
    {
        // Arrange
        var classIdWithStudents = this.testDb.Class1.Id; 

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._classService.DeleteClassAsync(classIdWithStudents));

        Assert.That(ex!.Message, Does.Contain("Cannot delete class with enrolled students."));
    }

    [Test]
    [Category("InvalidInput")]
    public void ShouldThrowException_WhenDeletingNonExistentClass()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._classService.DeleteClassAsync(nonExistentId));

        Assert.That(ex!.Message, Is.EqualTo("Class not found."));
    }
}

