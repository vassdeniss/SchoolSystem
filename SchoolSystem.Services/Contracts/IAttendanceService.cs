using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceDto>> GetAttendancesByStudentIdAsync(Guid id);
    Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id);
    Task CreateAttendanceAsync(AttendanceDto dto);
    Task UpdateAttendanceAsync(AttendanceDto dto);
    Task DeleteAttendanceAsync(Guid id);
}
