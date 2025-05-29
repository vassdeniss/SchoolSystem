using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class SchoolService(IRepository repository, ILogger<SchoolService> logger, IMapper mapper) : ISchoolService
{
    public async Task<IEnumerable<SchoolDto>> GetSchoolsAsync()
    {
        return await repository.AllReadonly<School>()
            .Include(s => s.Principal)
            .ThenInclude(p => p.User)
            .ProjectTo<SchoolDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
    
    public async Task<SchoolDto?> GetSchoolByIdAsync(Guid id)
    {
        return await repository.AllReadonly<School>()
            .Where(s => s.Id == id)
            .Include(s => s.Principal)
            .ThenInclude(p => p.User)
            .ProjectTo<SchoolDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateSchoolAsync(SchoolDto schoolDto)
    {
        IQueryable<School> schools = repository.AllReadonly<School>();
        if (await schools.AnyAsync(p => p.PrincipalId == schoolDto.PrincipalId))
        {
            throw new InvalidOperationException("Principal is already managing a school.");
        }
        
        School school = mapper.Map<School>(schoolDto);
        await repository.AddAsync(school);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateSchoolAsync(SchoolDto dto)
    {
        School? school = await repository.GetByIdAsync<School>(dto.Id);
        if (school == null)
        {
            throw new InvalidOperationException("School not found.");
        }

        school.Name = dto.Name;
        school.Address = dto.Address;
        school.PrincipalId = dto.PrincipalId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteSchoolAsync(Guid id)
    {
        IQueryable<Subject> subjects = repository.All<Subject>()
            .Where(s => s.SchoolId == id);
        repository.DeleteRange(subjects);
        
        await repository.DeleteAsync<School>(id);
        await repository.SaveChangesAsync();
    }
}
