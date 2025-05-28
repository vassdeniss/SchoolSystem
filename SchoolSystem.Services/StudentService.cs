using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class StudentService(IRepository repository, IMapper mapper) : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetStudentsByClassAsync(Guid classId)
    {
        return await repository.AllReadonly<Student>()
            .Where(s => s.ClassId == classId)
            .ProjectTo<StudentDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<StudentDto?> GetStudentAsync(Guid id)
    {
        return await repository.AllReadonly<Student>()
            .Where(s => s.Id == id)
            .ProjectTo<StudentDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();    
    }

    public async Task CreateStudentAsync(StudentDto studentDto)
    {
        IQueryable<Student> students = repository.AllReadonly<Student>();
        if (await students.AnyAsync(p => p.UserId == studentDto.UserId))
        {
            throw new InvalidOperationException("User is already a student.");
        }

        Student student = mapper.Map<Student>(studentDto);
        await repository.AddAsync(student);
        await repository.SaveChangesAsync();    
    }

    public async Task UpdateStudentAsync(StudentDto dto)
    {
        Student? student = await repository.GetByIdAsync<Student>(dto.Id);
        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        student.ClassId = dto.ClassId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(Guid id)
    {
        await repository.DeleteAsync<Student>(id);
        await repository.SaveChangesAsync();
    }
}
