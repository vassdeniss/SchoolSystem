using AutoMapper;
using SchoolSystem.Common;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.Principal;
using SchoolSystem.Web.Models.School;
using SchoolSystem.Web.Models.User;

namespace SchoolSystem.Web;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        this.CreateMap<UserDto, UserViewModel>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src
                    => $"{src.FirstName} {src.MiddleName} {src.LastName}"));
        this.CreateMap<UserDto, UserEditViewModel>();
        this.CreateMap<UserEditViewModel, UserDto>();

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
                    => $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}"));
        this.CreateMap<PrincipalEditViewModel, PrincipalDto>();
        
        this.CreateMap<SchoolDto, SchoolViewModel>()
            .ForMember(dest => dest.PrincipalName, 
                opt => opt.MapFrom(src => 
                    $"{src.Principal.User.FirstName} {src.Principal.User.MiddleName} {src.Principal.User.LastName}"));
        this.CreateMap<SchoolCreateViewModel, SchoolDto>();
        this.CreateMap<SchoolDto, SchoolEditViewModel>();
        this.CreateMap<SchoolEditViewModel, SchoolDto>();
    }    
}
