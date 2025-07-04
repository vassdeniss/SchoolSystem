using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Tests;

public class SubjectServiceTestBase : UnitTestBase
{
    protected SubjectService _subjectService;

    [SetUp]
    public void SetupService()
    {
        this._subjectService = new SubjectService(this.repo, this.mapper);
    }
}

[TestFixture]
public class GetSubjectsBySchoolIdAsyncTests : SubjectServiceTestBase
{
    [Test]
    public async Task ShouldReturnSubjectsForGivenSchool()
    {
        // Arrange
        Guid schoolId = this.testDb.School1.Id;

        // Act
        var subjects = await this._subjectService.GetSubjectsBySchoolIdAsync(schoolId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subjects, Is.Not.Null.And.Not.Empty, "Subjects should be returned");
            Assert.That(subjects.All(s => s.SchoolId == schoolId), Is.True, "All subjects should belong to the specified school");
            Assert.That(subjects.Any(s => s.Name == "Mathematics"), Is.True);
            Assert.That(subjects.Any(s => s.Name == "Chemistry"), Is.True);
        });
    }

    [Test]
    public async Task ShouldReturnEmptyList_WhenNoSubjectsInSchool()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid(); 

        // Act
        var subjects = await this._subjectService.GetSubjectsBySchoolIdAsync(schoolId);

        // Assert
        Assert.That(subjects, Is.Empty, "No subjects should be returned for unknown school");
    }
}

[TestFixture]
public class GetSubjectByIdAsyncTests : SubjectServiceTestBase
{
    [Test]
    public async Task ShouldReturnSubject_WhenValidIdProvided()
    {
        // Arrange
        Guid subjectId = this.testDb.Subject1.Id;

        // Act
        var subject = await this._subjectService.GetSubjectByIdAsync(subjectId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subject, Is.Not.Null, "Subject should be found");
            Assert.That(subject!.Id, Is.EqualTo(subjectId), "Returned subject should have matching ID");
            Assert.That(subject.Name, Is.EqualTo("Mathematics"), "Name should match expected subject");
        });
    }

    [Test]
    public async Task ShouldReturnNull_WhenSubjectDoesNotExist()
    {
        // Arrange
        Guid invalidId = Guid.NewGuid();

        // Act
        var subject = await this._subjectService.GetSubjectByIdAsync(invalidId);

        // Assert
        Assert.That(subject, Is.Null, "No subject should be returned for unknown ID");
    }

    [Test]
    public async Task ShouldReturnSubject_WhenItBelongsToDifferentSchool()
    {
        // Arrange
        Guid subjectId = this.testDb.Subject3.Id;

        // Act
        var subject = await this._subjectService.GetSubjectByIdAsync(subjectId);

        // Assert
        Assert.That(subject, Is.Not.Null, "Subject should be found even if from a different school");
        Assert.That(subject!.SchoolId, Is.EqualTo(this.testDb.School2.Id), "Subject should be correctly linked to School2");
        Assert.That(subject.Name, Is.EqualTo("History"), "Name should match the expected value");
    }
}

[TestFixture]
public class CreateSubjectAsyncTests : SubjectServiceTestBase
{
    [Test]
    public async Task ShouldCreateSubject_WhenValidDataProvided()
    {
        // Arrange
        var dto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = "Physics",
            SchoolId = this.testDb.School1.Id
        };

        // Act
        await this._subjectService.CreateSubjectAsync(dto);

        // Assert
        var subject = await this.repo.AllReadonly<Subject>()
            .FirstOrDefaultAsync(s => s.Id == dto.Id);

        Assert.Multiple(() =>
        {
            Assert.That(subject, Is.Not.Null, "Subject should be created");
            Assert.That(subject!.Name, Is.EqualTo(dto.Name), "Subject name should match");
            Assert.That(subject.SchoolId, Is.EqualTo(dto.SchoolId), "Subject should be linked to correct school");
        });
    }

    [Test]
    public async Task ShouldNotCreateSubject_WhenNameIsEmpty()
    {
        var dto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            SchoolId = this.testDb.School1.Id
        };

        // Act
        await this._subjectService.CreateSubjectAsync(dto);

        var result = await this.repo.AllReadonly<Subject>()
            .FirstOrDefaultAsync(s => s.Id == dto.Id);

        Assert.That(result, Is.Null, "Subject with empty name should not be created");
    }

    [Test]
    public void ShouldThrow_WhenSchoolDoesNotExist()
    {
        var dto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = "Philosophy",
            SchoolId = Guid.NewGuid()
        };

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._subjectService.CreateSubjectAsync(dto),
            "School not found.");
    }

    [Test]
    public async Task ShouldNotAllowDuplicateSubjectNameInSameSchool()
    {
        var dto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics",
            SchoolId = this.testDb.School1.Id
        };

        // Act
        await this._subjectService.CreateSubjectAsync(dto);

        var count = await this.repo.AllReadonly<Subject>()
            .CountAsync(s => s.Name == "Mathematics" && s.SchoolId == this.testDb.School1.Id);

        Assert.That(count, Is.EqualTo(1), "Duplicate subject name should not be created in the same school");
    }
}

[TestFixture]
public class UpdateSubjectAsyncTests : SubjectServiceTestBase
{
    [Test]
    public async Task ShouldUpdateSubject_WhenValidDataProvided()
    {
        // Arrange
        var original = this.testDb.Subject1;
        var dto = new SubjectDto
        {
            Id = original.Id,
            Name = "Advanced Mathematics",
            SchoolId = this.testDb.School2.Id
        };

        // Act
        await this._subjectService.UpdateSubjectAsync(dto);

        // Assert
        var updated = await this.repo.GetByIdAsync<Subject>(original.Id);

        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.Name, Is.EqualTo("Advanced Mathematics"));
            Assert.That(updated.SchoolId, Is.EqualTo(this.testDb.School2.Id));
        });
    }

    [Test]
    public void ShouldThrowException_WhenSubjectDoesNotExist()
    {
        // Arrange
        var dto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = "Biology",
            SchoolId = this.testDb.School1.Id
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this._subjectService.UpdateSubjectAsync(dto),
            "Subject not found.");
    }

    [Test]
    public async Task ShouldAllowUpdateWithSameData()
    {
        // Arrange
        var existing = this.testDb.Subject2;
        var dto = new SubjectDto
        {
            Id = existing.Id,
            Name = existing.Name,
            SchoolId = existing.SchoolId
        };

        // Act
        await this._subjectService.UpdateSubjectAsync(dto);

        // Assert
        var subject = await this.repo.GetByIdAsync<Subject>(existing.Id);
        Assert.That(subject, Is.Not.Null);
        Assert.That(subject!.Name, Is.EqualTo(existing.Name));
        Assert.That(subject.SchoolId, Is.EqualTo(existing.SchoolId));
    }
}

[TestFixture]
public class DeleteSubjectAsyncTests : SubjectServiceTestBase
{
    [Test]
    public async Task ShouldDeleteSubject_WhenValidIdProvided()
    {
        // Arrange
        var subjectId = this.testDb.Subject2.Id;

        // Act
        await this._subjectService.DeleteSubjectAsync(subjectId);

        // Assert
        var result = await this.repo.GetByIdAsync<Subject>(subjectId);
        Assert.That(result, Is.Null, "Subject should be deleted from the database");
    }

    [Test]
    public async Task ShouldNotThrow_WhenDeletingNonExistingSubject()
    {
        var id = Guid.NewGuid();
        await this._subjectService.DeleteSubjectAsync(id);

        Assert.Pass("No exception was thrown.");
    }
}








