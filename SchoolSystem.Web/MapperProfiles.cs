using AutoMapper;
using SchoolSystem.Common;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.Principal;

namespace SchoolSystem.Web;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        this.CreateMap<PrincipalDto, PrincipalViewModel>()
            .ForMember(dest => dest.FullName, 
                opt => opt.MapFrom(src 
                    => $"{src.User!.FirstName} {src.User.MiddleName} {src.User.LastName}"))
            .ForMember(dest => dest.SchoolName, 
                opt => opt.MapFrom(src => src.School!.Name));
        this.CreateMap<PrincipalCreateViewModel, PrincipalDto>();
        this.CreateMap<PrincipalDto, PrincipalEditViewModel>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src
                    => $"{src.User!.FirstName} {src.User.MiddleName} {src.User.LastName}"));
        this.CreateMap<PrincipalEditViewModel, PrincipalDto>();
    }    
}
