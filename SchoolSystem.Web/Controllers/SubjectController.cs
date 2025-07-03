using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class SubjectController(ISubjectService subjectService, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Create(Guid schoolId)
    {
        SubjectFormViewModel model = new()
        {
            SchoolId = schoolId
        };
        
        return this.View("Form", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubjectFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View("Form", model);
        }

        await subjectService.CreateSubjectAsync(mapper.Map<SubjectDto>(model));
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        SubjectDto? subject = await subjectService.GetSubjectByIdAsync(id);
        if (subject == null)
        {
            return this.NotFound();
        }

        SubjectFormViewModel viewModel = mapper.Map<SubjectFormViewModel>(subject);
        return this.View("Form", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SubjectFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View("Form", model);
        }

        SubjectDto? dto = mapper.Map<SubjectDto>(model);
        await subjectService.UpdateSubjectAsync(dto);
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Guid schoolId)
    {
        await subjectService.DeleteSubjectAsync(id);
        return this.RedirectToAction("Details", "School", new { id = schoolId });
    }
}
