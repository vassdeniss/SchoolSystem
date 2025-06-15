using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IParentService
{
    Task<IEnumerable<ParentDto>> GetAllParentsAsync();
    Task<ParentDto?> GetParentByIdAsync(Guid id);
    Task AddStudentToParentAsync(Guid parentId, Guid studentId);
    Task RemoveStudentFromParentAsync(Guid parentId, Guid studentId);
    Task CreateParentAsync(ParentDto dto);
    Task UpdateParentAsync(ParentDto dto);
    Task DeleteParentAsync(Guid id);
}
