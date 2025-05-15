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
        

        this.CreateMap<Principal, PrincipalDto>();
        this.CreateMap<PrincipalDto, Principal>();
    }    
}
