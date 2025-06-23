using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetTeachersBySchoolIdAsync(Guid id);
    Task<TeacherDto?> GetTeacherByIdAsync(Guid id);
    Task<bool> CanTeacherManageStudent(Guid teacherId, Guid studentId);
    Task CreateTeacherAsync(TeacherDto dto);
    Task UpdateTeacherAsync(TeacherDto dto);
    Task DeleteTeacherAsync(Guid id, Guid schoolId);
}
