using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Curriculum;
using SchoolSystem.Web.Models.Subject;
using SchoolSystem.Web.Models.Teacher;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator,Director")]
public class CurriculumController(ICurriculumService curriculumService, 
    ITeacherService teacherService,
    ISubjectService subjectService,
    IClassService classService,
    IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(Guid classId)
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
    
    [HttpGet]
    public async Task<IActionResult> Create(Guid classId, Guid schoolId)
    {
        (SelectList teachers, SelectList subjects) = await GetDropdownsData(schoolId);
        
        CurriculumFormViewModel model = new()
        {
            ClassId = classId,
            AvailableTeachers = teachers,
            AvailableSubjects = subjects
        };
        
        return this.View("Form", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CurriculumFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            (SelectList teachers, SelectList subjects) = await GetDropdownsData(model.SchoolId, model.TeacherId, model.SubjectId);
            model.AvailableTeachers = teachers;
            model.AvailableSubjects = subjects;
            return this.View("Form", model);
        }
    
        await curriculumService.CreateCurriculumAsync(mapper.Map<CurriculumDto>(model));
        return this.RedirectToAction("Curriculum", "Class", new { classId = model.ClassId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        CurriculumDto? dto = await curriculumService.GetCurriculumByIdAsync(id);
        if (dto == null)
        {
            return this.NotFound();
        }

        (SelectList teachers, SelectList subjects) = await GetDropdownsData(dto.Class.SchoolId, dto.TeacherId, dto.SubjectId);
        
        CurriculumFormViewModel viewModel = mapper.Map<CurriculumFormViewModel>(dto);
        viewModel.AvailableTeachers = teachers;
        viewModel.AvailableSubjects = subjects;
    
        return this.View("Form", viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CurriculumFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            (SelectList teachers, SelectList subjects) = await GetDropdownsData(model.SchoolId, model.TeacherId, model.SubjectId);
            model.AvailableTeachers = teachers;
            model.AvailableSubjects = subjects;
            return this.View("Form", model);
        }
    
        try 
        {
            CurriculumDto? dto = mapper.Map<CurriculumDto>(model);
            await curriculumService.UpdateCurriculumAsync(dto);
            return this.RedirectToAction("Curriculum", "Class", new { classId = model.ClassId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            (SelectList teachers, SelectList subjects) = await GetDropdownsData(model.SchoolId, model.TeacherId, model.SubjectId);
            model.AvailableTeachers = teachers;
            model.AvailableSubjects = subjects;
            return this.View("Form", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Guid classId)
    {
        await curriculumService.DeleteCurriculumAsync(id);
        return this.RedirectToAction("Curriculum", "Class", new { classId });
    }
    
    private async Task<(SelectList Teachers, SelectList Subjects)> GetDropdownsData(Guid schoolId, Guid? selectedTeacherId = null, Guid? selectedSubjectId = null)
    {
        IEnumerable<TeacherDto> teachers = await teacherService.GetTeachersBySchoolIdAsync(schoolId);
        IEnumerable<SubjectDto> subjects = await subjectService.GetSubjectsBySchoolIdAsync(schoolId);
        
        return (
            new SelectList(mapper.Map<IEnumerable<TeacherViewModel>>(teachers), "Id", "FullName", selectedTeacherId),
            new SelectList(mapper.Map<IEnumerable<SubjectViewModel>>(subjects), "Id", "Name", selectedSubjectId)
        );
    }
}
