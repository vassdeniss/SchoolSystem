using AutoMapper;
using SchoolSystem.Common;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        this.CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                $"{src.FirstName} {src.MiddleName} {src.LastName}"));
        
        CreateMap<Principal, PrincipalDto>()
            .ForMember(dest => dest.UserFullName, 
                opt => opt.MapFrom(src 
                    => $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}"))
            .ForMember(dest => dest.SchoolName, 
                opt => opt.MapFrom(src => src.School.Name));

        CreateMap<PrincipalCrudDto, Principal>();
    }    
}
