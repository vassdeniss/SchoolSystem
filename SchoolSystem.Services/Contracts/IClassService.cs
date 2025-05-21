using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IClassService
{
    Task<IEnumerable<ClassDto>> GetClassesAsync();
    Task<IEnumerable<ClassDto>> GetClassesBySchoolIdAsync(Guid id);
    Task<ClassDto?> GetClassByIdAsync(Guid id);
    Task CreateClassAsync(ClassDto classDto);
    Task UpdateClassAsync(ClassDto classDto);
    Task DeleteClassAsync(Guid id);
}
