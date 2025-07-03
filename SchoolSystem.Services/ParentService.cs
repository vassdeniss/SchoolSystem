using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services;

public class ParentService(IRepository repo, IMapper mapper) : IParentService
{
    public async Task<IEnumerable<ParentDto>> GetAllParentsAsync()
    {
        return await repo.AllReadonly<Parent>()
            .Include(p => p.Students)
            .ThenInclude(s => s.Class)
            .ProjectTo<ParentDto>(mapper.ConfigurationProvider)
            .ToListAsync();    
    }

    public async Task<ParentDto?> GetParentByIdAsync(Guid id)
    {
        return await repo.AllReadonly<Parent>()
            .Where(p => p.Id == id)
            .ProjectTo<ParentDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();    
    }

    public async Task AddStudentToParentAsync(Guid parentId, Guid studentId)
    {
        Parent parent = await repo.GetByIdAsync<Parent>(parentId);
        Student student = await repo.GetByIdAsync<Student>(studentId);
        parent.Students.Add(student);
        await repo.SaveChangesAsync();
    }
    
    public async Task RemoveStudentFromParentAsync(Guid parentId, Guid studentId)
    {
        Parent parent = await repo.All<Parent>(p => p.Id == parentId)
            .Include(p => p.Students)
            .FirstAsync();
        Student student = await repo.GetByIdAsync<Student>(studentId);
        parent.Students.Remove(student);
        await repo.SaveChangesAsync();
    }
    
    public async Task CreateParentAsync(ParentDto dto)
    {
        IQueryable<Parent> parents = repo.AllReadonly<Parent>();
        if (await parents.AnyAsync(p => p.UserId == dto.UserId))
        {
            throw new InvalidOperationException("User is already a parent.");
        }

        Parent parent = mapper.Map<Parent>(dto);
        await repo.AddAsync(parent);
        await repo.SaveChangesAsync();
    }

    public async Task UpdateParentAsync(ParentDto dto)
    {
        Parent? parent = await repo.GetByIdAsync<Parent>(dto.Id);
        if (parent == null)
        {
            throw new InvalidOperationException("Parent not found.");
        }

        parent.PhoneNumber = dto.PhoneNumber;
            
        await repo.SaveChangesAsync();
    }

    public async Task DeleteParentAsync(Guid id)
    {
        await repo.DeleteAsync<Parent>(id);
        await repo.SaveChangesAsync();
    }

}
