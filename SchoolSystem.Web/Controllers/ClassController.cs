using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Class;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator,Director")]
public class ClassController(IClassService classService, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Create(Guid schoolId)
    {
        ClassFormViewModel model = new()
        {
            SchoolId = schoolId
        };
        
        return this.View("Form", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClassFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View("Form", model);
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

        ClassFormViewModel viewModel = mapper.Map<ClassFormViewModel>(cls);
        return this.View("Form", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClassFormViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View("Form", model);
        }

        ClassDto? dto = mapper.Map<ClassDto>(model);
        await classService.UpdateClassAsync(dto);
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Guid schoolId)
    {
        await classService.DeleteClassAsync(id);
        return this.RedirectToAction("Details", "School", new { id = schoolId });
    }
}
