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
        SubjectCreateViewModel model = new()
        {
            SchoolId = schoolId
        };
        
        return this.View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubjectCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
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

        SubjectEditViewModel viewModel = mapper.Map<SubjectEditViewModel>(subject);
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SubjectEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        SubjectDto? dto = mapper.Map<SubjectDto>(model);
        await subjectService.UpdateSubjectAsync(dto);
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        SubjectDto? subject = await subjectService.GetSubjectByIdAsync(id);
        if (subject == null)
        {
            return this.NotFound();
        }

        return this.View(mapper.Map<SubjectViewModel>(subject));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, Guid schoolId)
    {
        await subjectService.DeleteSubjectAsync(id);
        return this.RedirectToAction("Details", "School", new { id = schoolId });
    }
}
