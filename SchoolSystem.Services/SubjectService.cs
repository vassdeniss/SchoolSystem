using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class SubjectService(IRepository repository, IMapper mapper) : ISubjectService
{
    public async Task<IEnumerable<SubjectDto>> GetSubjectsBySchoolIdAsync(Guid id)
    {
        return await repository.AllReadonly<Subject>()
            .Where(c => c.SchoolId == id)
            .ProjectTo<SubjectDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<SubjectDto?> GetSubjectByIdAsync(Guid id)
    {
        return await repository.AllReadonly<Subject>()
            .Where(c => c.Id == id)
            .ProjectTo<SubjectDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateSubjectAsync(SubjectDto subjectDto)
    {
        Subject subject = mapper.Map<Subject>(subjectDto);
        await repository.AddAsync(subject);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateSubjectAsync(SubjectDto dto)
    {
        Subject? subject = await repository.GetByIdAsync<Subject>(dto.Id);
        if (subject == null)
        {
            throw new InvalidOperationException("Subject not found.");
        }

        subject.Name = dto.Name;
        subject.SchoolId = dto.SchoolId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteSubjectAsync(Guid id)
    {
        await repository.DeleteAsync<Subject>(id);
        await repository.SaveChangesAsync();
    }
}
