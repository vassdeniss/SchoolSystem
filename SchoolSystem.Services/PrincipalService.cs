using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Common;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Contracts;

namespace SchoolSystem.Services;

public class PrincipalService(IRepository repo, IMapper mapper) : IPrincipalService
{
    public async Task<IEnumerable<PrincipalDto>> GetAllPrincipalsAsync()
    {
        return await repo.AllReadonly<Principal>()
            .ProjectTo<PrincipalDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PrincipalDto?> GetPrincipalByIdAsync(Guid id)
    {
        return await repo.AllReadonly<Principal>()
            .Where(p => p.Id == id)
            .ProjectTo<PrincipalDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task CreatePrincipalAsync(PrincipalDto dto)
    {
        IQueryable<Principal> principals = repo.AllReadonly<Principal>();
        if (await principals.AnyAsync(p => p.UserId == dto.UserId))
        {
            throw new InvalidOperationException("User is already a principal.");
        }

        Principal? principal = mapper.Map<Principal>(dto);
        await repo.AddAsync(principal);
        await repo.SaveChangesAsync();
    }

    public async Task UpdatePrincipalAsync(PrincipalDto dto)
    {
        Principal? principal = await repo.GetByIdAsync<Principal>(dto.Id);
        if (principal == null)
        {
            throw new InvalidOperationException("Principal not found.");
        }

        principal.Specialization = dto.Specialization;
        principal.PhoneNumber = dto.PhoneNumber;
            
        await repo.SaveChangesAsync();
    }

    public async Task DeletePrincipalAsync(Guid id)
    {
        await repo.DeleteAsync<Principal>(id);
        await repo.SaveChangesAsync();
    }
}
