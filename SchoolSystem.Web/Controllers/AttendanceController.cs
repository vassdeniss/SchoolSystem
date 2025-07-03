using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Extensions;
using SchoolSystem.Web.Models.Attendance;
using SchoolSystem.Web.Models.Student;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Controllers;

public class AttendanceController(
    IAttendanceService attendanceService,
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
        if (this.User.IsInRole("Student") && studentInfo.UserId != userId)
        {
            return this.Forbid();
        }
        
        if (this.User.IsInRole("Teacher"))
        {
            bool isTeaching = await teacherService.CanTeacherManageStudent(userId, studentId);
            if (!isTeaching) return this.Forbid();
        }

        StudentViewModel studentVm = mapper.Map<StudentViewModel>(studentInfo);
        IEnumerable<AttendanceDto> attendances = await attendanceService.GetAttendancesByStudentIdAsync(studentId);
        IEnumerable<AttendanceViewModel> attendancesVm = mapper.Map<IEnumerable<AttendanceViewModel>>(attendances);
        
        AttendanceListViewModel vm = new()
        {
            StudentId = studentId,
            StudentName = studentVm.FullName,
            SchoolId = studentVm.SchoolId,
            Attendances = attendancesVm
        };
        
        return this.View(vm);
    }
    
    [HttpGet]
    [Authorize(Roles = "Student,Parent,Director")]
    public async Task<IActionResult> Stats()
    {
        StudentDto? studentInfo = await studentService.GetStudentsByUserAsync(this.User.Id());
        if (studentInfo == null)
        {
            return this.NotFound();
        }

        StudentViewModel studentVm = mapper.Map<StudentViewModel>(studentInfo);
        IEnumerable<AttendanceDto> attendances = await attendanceService.GetAttendancesByStudentIdAsync(studentInfo.Id);
        IEnumerable<AttendanceViewModel> attendancesVm = mapper.Map<IEnumerable<AttendanceViewModel>>(attendances);
        
        AttendanceListViewModel vm = new()
        {
            StudentId = studentInfo.Id,
            StudentName = studentVm.FullName,
            SchoolId = studentVm.SchoolId,
            Attendances = attendancesVm
        };
        
        return this.View(nameof(Index), vm);
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
        AttendanceFormViewModel model = new()
        {
            StudentId = studentId,
            AvailableSubjects = subjects
        };
        
        return this.View("Form", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Create(AttendanceFormViewModel model)
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
    
        await attendanceService.CreateAttendanceAsync(mapper.Map<AttendanceDto>(model));
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
        
        AttendanceDto? dto = await attendanceService.GetAttendanceByIdAsync(id);
        if (dto == null)
        {
            return this.NotFound();
        }
    
        SelectList subjects = await GetDropdownsData(schoolId, dto.SubjectId);
        AttendanceFormViewModel viewModel = mapper.Map<AttendanceFormViewModel>(dto);
        viewModel.AvailableSubjects = subjects;
    
        return this.View("Form", viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Teacher,Director")]
    public async Task<IActionResult> Edit(AttendanceFormViewModel model)
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
            AttendanceDto? dto = mapper.Map<AttendanceDto>(model);
            await attendanceService.UpdateAttendanceAsync(dto);
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
        
        await attendanceService.DeleteAttendanceAsync(id);
        return this.RedirectToAction("Index", new { studentId });
    }
    
    private async Task<SelectList> GetDropdownsData(Guid schoolId, Guid? selectedSubjectId = null)
    {
        IEnumerable<SubjectDto> subjects = await subjectService.GetSubjectsBySchoolIdAsync(schoolId);
        return new SelectList(mapper.Map<IEnumerable<SubjectViewModel>>(subjects), "Id", "Name", selectedSubjectId);
    }
}
