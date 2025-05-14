using AutoMapper;
using SchoolSystem.Common;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        this.CreateMap<PrincipalCreateViewModel, PrincipalCrudDto>();
        this.CreateMap<PrincipalDto, PrincipalEditViewModel>();
        this.CreateMap<PrincipalEditViewModel, PrincipalCrudDto>();
    }    
}
