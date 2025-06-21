using AutoMapper;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services.Dtos;

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

        this.CreateMap<School, SchoolDto>();
        this.CreateMap<SchoolDto, School>();

        this.CreateMap<Class, ClassDto>();
        this.CreateMap<ClassDto, Class>();
        
        this.CreateMap<Student, StudentDto>();
        this.CreateMap<StudentDto, Student>();
        
        this.CreateMap<Subject, SubjectDto>();
        this.CreateMap<SubjectDto, Subject>();
        
        this.CreateMap<Teacher, TeacherDto>();
        this.CreateMap<TeacherDto, Teacher>();
        
        this.CreateMap<Parent, ParentDto>();
        this.CreateMap<ParentDto, Parent>();
        
        this.CreateMap<Curriculum, CurriculumDto>();
        this.CreateMap<CurriculumDto, Curriculum>();
    }    
}
