using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Extensions;
using SchoolSystem.Web.Models.Grade;
using SchoolSystem.Web.Models.Student;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Controllers;

public class GradeController(
    IGradeService gradeService,
    IStudentService studentService,
    ISubjectService subjectService,
    ITeacherService teacherService,
    IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Administrator,Teacher,Student,Director")]
    public async Task<IActionResult> Index(Guid studentId)
    {
        StudentDto? studentInfo = await studentService.GetStudentAsync(studentId);
        if (studentInfo == null)
        {
            return this.NotFound();
        }

        Guid userId = this.User.Id();
        if (this.User.IsInRole("Student") && studentId != userId)
        {
            return this.Forbid();
        }
        
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(userId, studentId);
            if (!isTeaching) return this.Forbid();
        }

        StudentViewModel studentVm = mapper.Map<StudentViewModel>(studentInfo);
        IEnumerable<GradeDto> grades = await gradeService.GetGradesByStudentIdAsync(studentId);
        IEnumerable<GradeViewModel> gradesVm = mapper.Map<IEnumerable<GradeViewModel>>(grades);
        
        GradeListViewModel vm = new()
        {
            StudentId = studentId,
            StudentName = studentVm.FullName,
            SchoolId = studentVm.SchoolId,
            Grades = gradesVm
        };
        
        return this.View(vm);
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Create(Guid studentId, Guid schoolId)
    {
        Guid currentUserId = this.User.Id();
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(currentUserId, studentId);
            if (!isTeaching) return this.Forbid();
        }
        
        SelectList subjects = await GetDropdownsData(schoolId);
        
        GradeFormViewModel model = new()
        {
            StudentId = studentId,
            AvailableSubjects = subjects
        };
        
        return this.View("Form", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Create(GradeFormViewModel model)
    {
        Guid currentUserId = this.User.Id();
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(currentUserId, model.StudentId);
            if (!isTeaching) return this.Forbid();
        }
        
        if (!this.ModelState.IsValid)
        {
            SelectList subjects = await GetDropdownsData(model.SchoolId, model.SubjectId);
            model.AvailableSubjects = subjects;
            return this.View("Form", model);
        }
    
        await gradeService.CreateGradeAsync(mapper.Map<GradeDto>(model));
        return this.RedirectToAction("Index", new { studentId = model.StudentId });
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Edit(Guid id, Guid schoolId, Guid studentId)
    {
        Guid currentUserId = this.User.Id();
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(currentUserId, studentId);
            if (!isTeaching) return this.Forbid();
        }
        
        GradeDto? dto = await gradeService.GetGradeByIdAsync(id);
        if (dto == null)
        {
            return this.NotFound();
        }
    
        SelectList subjects = await GetDropdownsData(schoolId, dto.SubjectId);
        
        GradeFormViewModel viewModel = mapper.Map<GradeFormViewModel>(dto);
        viewModel.AvailableSubjects = subjects;
    
        return this.View("Form", viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Edit(GradeFormViewModel model)
    {
        Guid currentUserId = this.User.Id();
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(currentUserId, model.StudentId);
            if (!isTeaching) return this.Forbid();
        }
        
        if (!this.ModelState.IsValid)
        {
            SelectList subjects = await GetDropdownsData(model.SchoolId, model.SubjectId);
            model.AvailableSubjects = subjects;
            return this.View("Form", model);
        }
    
        try
        {
            GradeDto? dto = mapper.Map<GradeDto>(model);
            await gradeService.UpdateGradeAsync(dto);
            return this.RedirectToAction("Index", new { studentId = model.StudentId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            model.AvailableSubjects = await GetDropdownsData(model.SchoolId, model.SubjectId);
            return this.View("Form", model);
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Delete(Guid id, Guid studentId)
    {
        Guid currentUserId = this.User.Id();
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(currentUserId, studentId);
            if (!isTeaching) return this.Forbid();
        }
        
        await gradeService.DeleteGradeAsync(id);
        return this.RedirectToAction("Index", new { studentId });
    }
    
    private async Task<SelectList> GetDropdownsData(Guid schoolId, Guid? selectedSubjectId = null)
    {
        IEnumerable<SubjectDto> subjects = await subjectService.GetSubjectsBySchoolIdAsync(schoolId);
        return new SelectList(mapper.Map<IEnumerable<SubjectViewModel>>(subjects), "Id", "Name", selectedSubjectId);
    }
}
