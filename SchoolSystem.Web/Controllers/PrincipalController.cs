using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Common;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class PrincipalController(IUserService userService, IPrincipalService principalService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<PrincipalDto> principals = await principalService.GetAllPrincipalsAsync();
        return this.View(principals);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        IEnumerable<UserDto> principals = await userService.GetUsersWithRoleAsync("Director");
        
        PrincipalCreateViewModel viewModel = new()
        {
            AvailablePrincipals = new SelectList(principals, "Id", "FullName")
        };
    
        return this.View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrincipalCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailablePrincipals = new SelectList(await userService.GetUsersWithRoleAsync("Director"), "Id", "FullName");
            return this.View(model);
        }

        try
        {
            await principalService.CreatePrincipalAsync(mapper.Map<PrincipalCrudDto>(model));
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            model.AvailablePrincipals = new SelectList(await userService.GetUsersWithRoleAsync("Director"), "Id", "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        PrincipalDto? principal = await principalService.GetPrincipalByIdAsync(id);
        if (principal == null)
        {
            return this.NotFound();
        }

        PrincipalEditViewModel viewModel = mapper.Map<PrincipalEditViewModel>(principal);
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PrincipalEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        try
        {
            PrincipalCrudDto? dto = mapper.Map<PrincipalCrudDto>(model);
            await principalService.UpdatePrincipalAsync(dto);
            return this.RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        PrincipalDto? principal = await principalService.GetPrincipalByIdAsync(id);
        if (principal == null)
        {
            return this.NotFound();
        }

        return this.View(principal);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await principalService.DeletePrincipalAsync(id);
        return this.RedirectToAction(nameof(Index));
    }
}
