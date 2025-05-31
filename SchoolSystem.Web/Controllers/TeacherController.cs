using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Teacher;
using SchoolSystem.Web.Models.User;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class TeacherController(
    ITeacherService teacherService, 
    ISchoolService schoolService, 
    IUserService userService, 
    IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(Guid schoolId)
    {
        SchoolDto? school = await schoolService.GetSchoolByIdAsync(schoolId);
        if (school == null)
        {
            return this.NotFound();
        }

        IEnumerable<UserDto> teacherUsers = await userService.GetUsersWithRoleAsync("Teacher");
        IEnumerable<UserViewModel> teacherUserViewModels = mapper.Map<IEnumerable<UserViewModel>>(teacherUsers);
        
        TeacherCreateViewModel model = new()
        {
            SchoolId = school.Id,
            AvailableTeachers = new SelectList(teacherUserViewModels, "Id", "FullName"),
        };

        return this.View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeacherCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailableTeachers =
                new SelectList(
                    mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Teacher")), "Id",
                    "FullName");
            return this.View(model);
        }
    
        try
        {
            TeacherDto teacherDto = mapper.Map<TeacherDto>(model);
            await teacherService.CreateTeacherAsync(teacherDto);
            return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
        }
        catch (InvalidOperationException ex)
        {
            model.AvailableTeachers =
                new SelectList(
                    mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Teacher")), "Id",
                    "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, Guid schoolId)
    {
        TeacherDto? teacher = await teacherService.GetTeacherByIdAsync(id);
        if (teacher == null)
        {
            return this.NotFound();
        }
    
        TeacherEditViewModel viewModel = mapper.Map<TeacherEditViewModel>(teacher);
        viewModel.SchoolId = schoolId;
        return this.View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TeacherEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }
    
        TeacherDto? dto = mapper.Map<TeacherDto>(model);
        await teacherService.UpdateTeacherAsync(dto);
        return this.RedirectToAction("Details", "School", new { id = model.SchoolId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, Guid schoolId)
    {
        TeacherDto? teacher = await teacherService.GetTeacherByIdAsync(id);
        if (teacher == null)
        {
            return this.NotFound();
        }

        this.ViewBag.SchoolId = schoolId;
        
        return this.View(mapper.Map<TeacherViewModel>(teacher));
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, Guid schoolId)
    {
        await teacherService.DeleteTeacherAsync(id, schoolId);
        return this.RedirectToAction("Details", "School", new { id = schoolId });
    }
}
