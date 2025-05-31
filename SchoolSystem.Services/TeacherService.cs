using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class TeacherService(IRepository repository, IMapper mapper) : ITeacherService
{
    public async Task<IEnumerable<TeacherDto>> GetTeachersBySchoolIdAsync(Guid id)
    {
        return await repository.AllReadonly<Teacher>()
            .Where(t => t.Schools.Any(s => s.Id == id))
            .ProjectTo<TeacherDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
    {
        return await repository.AllReadonly<Teacher>()
            .Where(c => c.Id == id)
            .ProjectTo<TeacherDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateTeacherAsync(TeacherDto dto)
    {
        School school = await repository.GetByIdAsync<School>(dto.SchoolId);
        
        Teacher? existingTeacher = await repository.All<Teacher>()
            .Where(t => t.UserId == dto.UserId)
            .FirstOrDefaultAsync();
        if (existingTeacher is not null)
        {
            existingTeacher.Schools.Add(school);
        }
        else
        {
            Teacher teacher = mapper.Map<Teacher>(dto);
            teacher.Schools.Add(school);
            await repository.AddAsync(teacher);
        }
        
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateTeacherAsync(TeacherDto dto)
    {
        Teacher? teacher = await repository.GetByIdAsync<Teacher>(dto.Id);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }

        teacher.Specialization = dto.Specialization;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteTeacherAsync(Guid id, Guid schoolId)
    {
        Teacher teacher = await repository.All<Teacher>()
            .Include(t => t.Schools)
            .Where(t => t.Id == id)
            .FirstAsync();
        if (teacher.Schools.Count == 1)
        {
            await repository.DeleteAsync<Teacher>(id);
        }
        else
        {
            School school = await repository.GetByIdAsync<School>(schoolId);
            teacher.Schools.Remove(school);
        }

        await repository.SaveChangesAsync();
    }
}
