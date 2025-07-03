using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class AttendanceService(IRepository repository, IMapper mapper) : IAttendanceService
{
    public async Task<IEnumerable<AttendanceDto>> GetAttendancesByStudentIdAsync(Guid id)
    {
        return await repository.AllReadonly<Attendance>(g => g.StudentId == id)
            .ProjectTo<AttendanceDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id)
    {
        return await repository.AllReadonly<Attendance>()
            .Where(a => a.Id == id)
            .ProjectTo<AttendanceDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAttendanceAsync(AttendanceDto attendanceDto)
    {
        Attendance attendance = mapper.Map<Attendance>(attendanceDto);
        await repository.AddAsync(attendance);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateAttendanceAsync(AttendanceDto dto)
    {
        Attendance? attendance = await repository.GetByIdAsync<Attendance>(dto.Id);
        if (attendance == null)
        {
            throw new InvalidOperationException("Attendance not found.");
        }
        
        attendance.AbsenceType = dto.AbsenceType;
        attendance.SubjectId = dto.SubjectId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteAttendanceAsync(Guid id)
    {
        await repository.DeleteAsync<Attendance>(id);
        await repository.SaveChangesAsync();
    }
}
