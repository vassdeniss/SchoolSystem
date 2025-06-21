using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Class;
using SchoolSystem.Web.Models.Curriculum;
using SchoolSystem.Web.Models.Student;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator,Director")]
public class ClassController(IClassService classService, IStudentService studentService, ICurriculumService curriculumService, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Create(Guid schoolId)
    {
        ClassCreateViewModel model = new()
        {
            SchoolId = schoolId
        };
        
        return this.View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClassCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await classService.CreateClassAsync(mapper.Map<ClassDto>(model));
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        ClassDto? cls = await classService.GetClassByIdAsync(id);
        if (cls == null)
        {
            return this.NotFound();
        }

        ClassEditViewModel viewModel = mapper.Map<ClassEditViewModel>(cls);
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClassEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        ClassDto? dto = mapper.Map<ClassDto>(model);
        await classService.UpdateClassAsync(dto);
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        ClassDto? cls = await classService.GetClassByIdAsync(id);
        if (cls == null)
        {
            return this.NotFound();
        }

        return this.View(mapper.Map<ClassViewModel>(cls));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, Guid schoolId)
    {
        await classService.DeleteClassAsync(id);
        return this.RedirectToAction("Details", "School", new { id = schoolId });
    }

    [HttpGet]
    public async Task<IActionResult> Students(Guid id, Guid schoolId)
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
    public async Task<IActionResult> Curriculum(Guid classId)
    {
        ClassDto? classInfo = await classService.GetClassByIdAsync(classId);
        if (classInfo == null)
        {
            return this.NotFound();
        }
        
        IEnumerable<CurriculumDto> curriculum = await curriculumService.GetCurriculumsByClassIdAsync(classId);
        CurriculumListViewModel viewModel = new()
        {
            ClassId = classId,
            ClassName = classInfo.Name,
            SchoolId = classInfo.SchoolId,
            Curriculum = mapper.Map<IEnumerable<CurriculumViewModel>>(curriculum)
        };
        
        return this.View(viewModel);
    }
}
