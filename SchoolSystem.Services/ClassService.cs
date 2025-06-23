using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class ClassService(IRepository repository, IMapper mapper) : IClassService
{
    public async Task<IEnumerable<ClassDto>> GetClassesBySchoolIdAsync(Guid id)
    {
        return await repository.AllReadonly<Class>()
            .Where(c => c.SchoolId == id)
            .OrderByDescending(c => c.Year)
            .ProjectTo<ClassDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ClassDto?> GetClassByIdAsync(Guid id)
    {
        return await repository.AllReadonly<Class>()
            .Where(c => c.Id == id)
            .ProjectTo<ClassDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateClassAsync(ClassDto classDto)
    {
        Class @class = mapper.Map<Class>(classDto);
        await repository.AddAsync(@class);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateClassAsync(ClassDto dto)
    {
        Class? @class = await repository.GetByIdAsync<Class>(dto.Id);
        if (@class == null)
        {
            throw new InvalidOperationException("Class not found.");
        }

        @class.Name = dto.Name;
        @class.Year = dto.Year;
        @class.Term = dto.Term;
        @class.SchoolId = dto.SchoolId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteClassAsync(Guid id)
    {
        await repository.DeleteAsync<Class>(id);
        await repository.SaveChangesAsync();
    }
}
