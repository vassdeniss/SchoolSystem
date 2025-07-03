using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Student;
using SchoolSystem.Web.Models.User;

namespace SchoolSystem.Web.Controllers;

public class StudentController(IStudentService studentService, 
    IClassService classService, 
    IUserService userService, 
    IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Administrator,Director,Teacher")]
    public async Task<IActionResult> Index(Guid id, Guid schoolId)
    {
        ClassDto? classInfo = await classService.GetClassByIdAsync(id);
        if (classInfo == null)
        {
            return this.NotFound();
        }
        
        IEnumerable<StudentDto> students = await studentService.GetStudentsByClassAsync(id);
        StudentListViewModel viewModel = new()
        {
            Id = id,
            SchoolId = schoolId,
            ClassName = classInfo.Name,
            Year = classInfo.Year,
            Term = classInfo.Term,
            Students = mapper.Map<IEnumerable<StudentViewModel>>(students)
        };
        
        return this.View(viewModel);
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(Guid schoolId, Guid classId)
    {
        IEnumerable<UserDto> students = await userService.GetUsersWithRoleAsync("Student");
        IEnumerable<UserViewModel> studentsVm = mapper.Map<IEnumerable<UserViewModel>>(students);
        
        StudentCreateViewModel viewModel = new()
        {
            SchoolId = schoolId,
            ClassId = classId,
            AvailableStudents = new SelectList(studentsVm, "Id", "FullName")
        };
    
        return this.View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(StudentCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailableStudents = new SelectList(mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Student")), "Id", "FullName");
            return this.View(model);
        }

        try
        {
            await studentService.CreateStudentAsync(mapper.Map<StudentDto>(model));
            return this.RedirectToAction("Students", "Class", new { id = model.ClassId, schoolId = model.SchoolId });
        }
        catch (InvalidOperationException ex)
        {
            model.AvailableStudents = new SelectList(mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Student")), "Id", "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator,Director,Teacher")]
    public async Task<IActionResult> Move(Guid id, Guid schoolId)
    {
        StudentDto? student = await studentService.GetStudentAsync(id);
        if (student == null)
        {
            return this.NotFound();
        }

        IEnumerable<ClassDto> availableClasses = await classService.GetClassesBySchoolIdAsync(schoolId);
        StudentMoveViewModel viewModel = new()
        {
            Id = student.Id,
            StudentName = $"{student.User.FirstName} {student.User.MiddleName} {student.User.LastName}",
            CurrentClassId = student.ClassId,
            SchoolId = schoolId,
            AvailableClasses = availableClasses
                .Where(c => c.Id != student.ClassId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} ({c.Term} {c.Year})"
                }).ToList()
        };

        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Director,Teacher")]
    public async Task<IActionResult> Move(StudentMoveViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailableClasses = await GetAvailableClasses(model.SchoolId, model.CurrentClassId);
            return View(model);
        }
    
        try
        {
            StudentDto dto = mapper.Map<StudentDto>(model);
            await studentService.UpdateStudentAsync(dto);
            return this.RedirectToAction("Students", "Class", new { id = model.CurrentClassId, schoolId = model.SchoolId });
        }
        catch (InvalidOperationException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            model.AvailableClasses = await GetAvailableClasses(model.SchoolId, model.CurrentClassId);
            return this.View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator,Director,Teacher")]
    public async Task<IActionResult> Delete(Guid id, Guid studentId)
    {
        await studentService.DeleteStudentAsync(id);
        return this.RedirectToAction("Details", "School", new { id = studentId });
    }
    
    private async Task<List<SelectListItem>> GetAvailableClasses(Guid schoolId, Guid currentClassId)
    {
        IEnumerable<ClassDto> classes = await classService.GetClassesBySchoolIdAsync(schoolId);
        return classes
            .Where(c => c.Id != currentClassId)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Name} ({c.Term} {c.Year})"
            }).ToList();
    }
}
