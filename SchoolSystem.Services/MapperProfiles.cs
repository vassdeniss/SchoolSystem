using AutoMapper;
using SchoolSystem.Common;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        this.CreateMap<User, UserDto>();
        this.CreateMap<UserDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        this.CreateMap<Principal, PrincipalDto>();
        this.CreateMap<PrincipalDto, Principal>();
    }    
}
