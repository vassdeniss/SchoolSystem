using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetStudentsByClassAsync(Guid classId);
    Task<StudentDto?> GetStudentsByUserAsync(Guid userId);
    Task<StudentDto?> GetStudentAsync(Guid id);
    Task<IEnumerable<StudentDto>> GetStudentsAssignedToUserAsync(Guid userId);
    Task<IEnumerable<StudentDto>> GetStudentsNotAssignedToParentAsync(Guid parentId);
    Task CreateStudentAsync(StudentDto studentDto);
    Task UpdateStudentAsync(StudentDto dto);
    Task DeleteStudentAsync(Guid id);
}
