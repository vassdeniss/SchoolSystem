using AutoMapper;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.Class;
using SchoolSystem.Web.Models.Principal;
using SchoolSystem.Web.Models.School;
using SchoolSystem.Web.Models.Student;
using SchoolSystem.Web.Models.Subject;
using SchoolSystem.Web.Models.Teacher;
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
        this.CreateMap<SchoolDto, SchoolDetailsViewModel>()
            .ForMember(dest => dest.PrincipalName,
                opt => opt.MapFrom(src =>
                    $"{src.Principal.User.FirstName} {src.Principal.User.MiddleName} {src.Principal.User.LastName}"));

        this.CreateMap<ClassDto, ClassViewModel>();
        this.CreateMap<ClassCreateViewModel, ClassDto>();
        this.CreateMap<ClassDto, ClassEditViewModel>();
        this.CreateMap<ClassEditViewModel, ClassDto>();

        this.CreateMap<StudentDto, StudentViewModel>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src =>
                    $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}"))
            .ForMember(dest => dest.Dob,
                opt => opt.MapFrom(src =>
                    src.User.DateOfBirth));
        this.CreateMap<StudentCreateViewModel, StudentDto>();
        this.CreateMap<StudentDto, StudentMoveViewModel>();
        this.CreateMap<StudentMoveViewModel, StudentDto>();
        
        this.CreateMap<SubjectDto, SubjectViewModel>();
        this.CreateMap<SubjectCreateViewModel, SubjectDto>();
        this.CreateMap<SubjectDto, SubjectEditViewModel>();
        this.CreateMap<SubjectEditViewModel, SubjectDto>();
        
        this.CreateMap<TeacherDto, TeacherViewModel>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src =>
                    $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}"));
        this.CreateMap<TeacherCreateViewModel, TeacherDto>();
        this.CreateMap<TeacherDto, TeacherEditViewModel>();
        this.CreateMap<TeacherEditViewModel, TeacherDto>();
    }    
}
