using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class CurriculumService(IRepository repository, IMapper mapper) : ICurriculumService
{
    public async Task<IEnumerable<CurriculumDto>> GetCurriculumsByClassIdAsync(Guid id)
    {
        return await repository.AllReadonly<Curriculum>()
            .Where(c => c.ClassId == id)
            .OrderBy(c => 
                c.DayOfWeek == "Понеделник" ? 1 :
                c.DayOfWeek == "Вторник" ? 2 :
                c.DayOfWeek == "Сряда" ? 3 :
                c.DayOfWeek == "Четвъртък" ? 4 :
                c.DayOfWeek == "Петък" ? 5 :
                c.DayOfWeek == "Събота" ? 6 :
                c.DayOfWeek == "Неделя" ? 7 : 8)
            .ThenBy(c => c.StartTime)
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .ProjectTo<CurriculumDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<CurriculumDto?> GetCurriculumByIdAsync(Guid id)
    {
        return await repository.AllReadonly<Curriculum>()
            .Where(c => c.Id == id)
            .ProjectTo<CurriculumDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreateCurriculumAsync(CurriculumDto curriculumDto)
    {
        Curriculum curriculum = mapper.Map<Curriculum>(curriculumDto);
        await repository.AddAsync(curriculum);
        await repository.SaveChangesAsync();
    }
    
    public async Task UpdateCurriculumAsync(CurriculumDto dto)
    {
        Curriculum? curriculum = await repository.GetByIdAsync<Curriculum>(dto.Id);
        if (curriculum == null)
        {
            throw new InvalidOperationException("Curriculum not found.");
        }
        
        curriculum.DayOfWeek = dto.DayOfWeek;
        curriculum.StartTime = dto.StartTime;
        curriculum.EndTime = dto.EndTime;
        curriculum.TeacherId = dto.TeacherId;
        curriculum.SubjectId = dto.SubjectId;
            
        await repository.SaveChangesAsync();
    }

    public async Task DeleteCurriculumAsync(Guid id)
    {
        await repository.DeleteAsync<Curriculum>(id);
        await repository.SaveChangesAsync();
    }
}
