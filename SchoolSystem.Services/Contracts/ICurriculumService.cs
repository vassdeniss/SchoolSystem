using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface ICurriculumService
{
    Task<IEnumerable<CurriculumDto>> GetCurriculumsByClassIdAsync(Guid id);
    Task<CurriculumDto?> GetCurriculumByIdAsync(Guid id);
    Task CreateCurriculumAsync(CurriculumDto curriculumDto);
    Task UpdateCurriculumAsync(CurriculumDto curriculumDto);
    Task DeleteCurriculumAsync(Guid id);
}
