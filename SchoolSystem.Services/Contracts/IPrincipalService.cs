using SchoolSystem.Services.Dtos;

namespace SchoolSystem.Services.Contracts;

public interface IPrincipalService
{
    Task<IEnumerable<PrincipalDto>> GetAllPrincipalsAsync();
    Task<PrincipalDto?> GetPrincipalByIdAsync(Guid id);
    Task CreatePrincipalAsync(PrincipalDto principalDto);
    Task UpdatePrincipalAsync(PrincipalDto principalDto);
    Task DeletePrincipalAsync(Guid id);
}
