using SchoolSystem.Common;

namespace SchoolSystem.Services.Contracts;

public interface IPrincipalService
{
    Task<IEnumerable<PrincipalDto>> GetAllPrincipalsAsync();
    Task<PrincipalDto?> GetPrincipalByIdAsync(Guid id);
    Task CreatePrincipalAsync(PrincipalCrudDto principalDto);
    Task UpdatePrincipalAsync(PrincipalCrudDto principalDto);
    Task DeletePrincipalAsync(Guid id);
}
