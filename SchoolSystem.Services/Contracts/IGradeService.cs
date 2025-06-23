using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IGradeService
{
    Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(Guid id);
    Task<GradeDto?> GetGradeByIdAsync(Guid id);
    Task CreateGradeAsync(GradeDto dto);
    Task UpdateGradeAsync(GradeDto dto);
    Task DeleteGradeAsync(Guid id);
}
