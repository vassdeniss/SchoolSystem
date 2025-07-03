using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class GradeService(IRepository repository, IMapper mapper) : IGradeService
{
    public async Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Student ID cannot be empty");

        return await repository.AllReadonly<Grade>(g => g.StudentId == id)
            .OrderByDescending(g => g.GradeDate)
            .ProjectTo<GradeDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<GradeDto?> GetGradeByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Grade ID cannot be empty");

        return await repository.AllReadonly<Grade>()
            .Where(c => c.Id == id)
            .ProjectTo<GradeDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateGradeAsync(GradeDto gradeDto)
    {
        Grade grade = mapper.Map<Grade>(gradeDto);
        await repository.AddAsync(grade);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateGradeAsync(GradeDto dto)
    {
        Grade? grade = await repository.GetByIdAsync<Grade>(dto.Id);
        if (grade == null)
        {
            throw new InvalidOperationException("Grade not found.");
        }
        
        grade.GradeDate = dto.GradeDate;
        grade.GradeValue = dto.GradeValue;
        grade.SubjectId = dto.SubjectId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteGradeAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Provided grade ID is empty");

        var grade = await repository.GetByIdAsync<Grade>(id);
        if (grade == null)
            throw new InvalidOperationException($"Entity of type Grade with id {id} could not be found");

        await repository.DeleteAsync<Grade>(id);
        await repository.SaveChangesAsync();
    }
}
