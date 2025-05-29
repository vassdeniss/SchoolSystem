using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetSubjectsBySchoolIdAsync(Guid id);
    Task<SubjectDto?> GetSubjectByIdAsync(Guid id);
    Task CreateSubjectAsync(SubjectDto subjectDto);
    Task UpdateSubjectAsync(SubjectDto subjectDto);
    Task DeleteSubjectAsync(Guid id);    
}
