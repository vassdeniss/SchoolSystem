﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Principal;
using SchoolSystem.Web.Models.School;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class SchoolController(ISchoolService schoolService, 
    IPrincipalService principalService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<SchoolDto> schools = await schoolService.GetSchoolsAsync();
        IEnumerable<SchoolViewModel> schoolsVm = mapper.Map<IEnumerable<SchoolViewModel>>(schools);
        return this.View(schoolsVm);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        SchoolDto? school = await schoolService.GetSchoolByIdAsync(id);
        if (school == null)
        {
            return this.NotFound();
        }
        
        SchoolDetailsViewModel viewModel = mapper.Map<SchoolDetailsViewModel>(school);

        return this.View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        IEnumerable<PrincipalDto> principals = await principalService.GetAllPrincipalsAsync();
        IEnumerable<PrincipalViewModel> principalsVm = mapper.Map<IEnumerable<PrincipalViewModel>>(principals);
        
        SchoolCreateViewModel viewModel = new()
        {
            AvailablePrincipals = new SelectList(principalsVm, "Id", "FullName")
        };
        
        return this.View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SchoolCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailablePrincipals = new SelectList(mapper.Map<IEnumerable<PrincipalViewModel>>(await principalService.GetAllPrincipalsAsync()), "Id", "FullName");
            return this.View(model);
        }

        try
        {
            SchoolDto schoolDto = mapper.Map<SchoolDto>(model);
            await schoolService.CreateSchoolAsync(schoolDto);
            return this.RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            model.AvailablePrincipals = new SelectList(mapper.Map<IEnumerable<PrincipalViewModel>>(await principalService.GetAllPrincipalsAsync()), "Id", "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        SchoolDto? school = await schoolService.GetSchoolByIdAsync(id);
        if (school == null)
        {
            return this.NotFound();
        }

        SchoolEditViewModel viewModel = mapper.Map<SchoolEditViewModel>(school);
        viewModel.AvailablePrincipals = new SelectList(mapper.Map<IEnumerable<PrincipalViewModel>>(await principalService.GetAllPrincipalsAsync()), "Id", "FullName");
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SchoolEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailablePrincipals = new SelectList(mapper.Map<IEnumerable<PrincipalViewModel>>(await principalService.GetAllPrincipalsAsync()), "Id", "FullName");
            return this.View(model);
        }

        try
        {
            SchoolDto? dto = mapper.Map<SchoolDto>(model);
            await schoolService.UpdateSchoolAsync(dto);
            return this.RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            model.AvailablePrincipals = new SelectList(mapper.Map<IEnumerable<PrincipalViewModel>>(await principalService.GetAllPrincipalsAsync()), "Id", "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        SchoolDto? school = await schoolService.GetSchoolByIdAsync(id);
        if (school == null)
        {
            return this.NotFound();
        }

        return this.View(mapper.Map<SchoolViewModel>(school));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await schoolService.DeleteSchoolAsync(id);
        return this.RedirectToAction(nameof(Index));
    }
}
