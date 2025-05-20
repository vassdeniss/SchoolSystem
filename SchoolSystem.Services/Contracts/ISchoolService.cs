using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface ISchoolService
{
    Task<IEnumerable<SchoolDto>> GetSchoolsAsync();
    Task<SchoolDto?> GetSchoolByIdAsync(Guid id);
    Task CreateSchoolAsync(SchoolDto schoolDto);
    Task UpdateSchoolAsync(SchoolDto schoolDto);
    Task DeleteSchoolAsync(Guid id);
}
