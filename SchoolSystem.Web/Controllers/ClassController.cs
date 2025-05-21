using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Class;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class ClassController(IClassService classService, IMapper mapper) : Controller
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
}
