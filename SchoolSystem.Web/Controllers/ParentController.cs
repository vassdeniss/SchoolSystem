using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Extensions;
using SchoolSystem.Web.Models.Parent;
using SchoolSystem.Web.Models.Student;
using SchoolSystem.Web.Models.User;

namespace SchoolSystem.Web.Controllers;

public class ParentController(IParentService parentService, IStudentService studentService, IUserService userService, IMapper mapper) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Index()
    {
        IEnumerable<ParentDto> parentDtos = await parentService.GetAllParentsAsync();
        return this.View(mapper.Map<IEnumerable<ParentViewModel>>(parentDtos));
    }
    
    [HttpGet]
    [Authorize(Roles = "Parent")]
    public async Task<IActionResult> Children()
    {
        Guid userId = this.User.Id();
        IEnumerable<StudentDto> studentDtos = await studentService.GetStudentsAssignedToUserAsync(userId);
        return this.View(mapper.Map<IEnumerable<StudentViewModel>>(studentDtos));
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create()
    {
        IEnumerable<UserDto> parents = await userService.GetUsersWithRoleAsync("Parent");
        IEnumerable<UserViewModel> parentsVm = mapper.Map<IEnumerable<UserViewModel>>(parents);

        ParentCreateViewModel viewModel = new()
        {
            AvailableParents = new SelectList(parentsVm, "Id", "FullName"),
        };
        
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(ParentCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.AvailableParents = new SelectList(mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Parent")), "Id", "FullName");
            return this.View(model);
        }

        try
        {
            await parentService.CreateParentAsync(mapper.Map<ParentDto>(model));
            return this.RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            model.AvailableParents = new SelectList(mapper.Map<IEnumerable<UserViewModel>>(await userService.GetUsersWithRoleAsync("Parent")), "Id", "FullName");
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        ParentDto? parentDto = await parentService.GetParentByIdAsync(id.Value);
        if (parentDto == null)
        {
            return this.NotFound();
        }

        return this.View(mapper.Map<ParentEditViewModel>(parentDto));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(ParentEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        try
        {
            await parentService.UpdateParentAsync(mapper.Map<ParentDto>(model));
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(model);
        }
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddStudent(Guid parentId)
    {
        ParentDto? parent = await parentService.GetParentByIdAsync(parentId);
        if (parent == null)
        {
            return this.NotFound();
        }

        IEnumerable<StudentDto> studentDtos = await studentService.GetStudentsNotAssignedToParentAsync(parentId);
        AddStudentToParentViewModel model = new()
        {
            ParentId = parentId,
            AvailableStudents = new SelectList(
                mapper.Map<IEnumerable<StudentViewModel>>(studentDtos),
                "Id", 
                "FullName")
        };

        return this.View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddStudent(AddStudentToParentViewModel model)
    {
        ParentDto? parent = await parentService.GetParentByIdAsync(model.ParentId);
        if (parent == null || (parent.UserId != this.User.Id() && !this.User.IsInRole("Administrator")))
        {
            return this.Forbid();
        }
        
        if (!this.ModelState.IsValid)
        {
            IEnumerable<StudentDto> studentDtos = await studentService.GetStudentsNotAssignedToParentAsync(model.ParentId);
            model.AvailableStudents = new SelectList(
                mapper.Map<IEnumerable<StudentViewModel>>(studentDtos),
                "Id", 
                "FullName");
            return this.View(model);
        }

        try
        {
            await parentService.AddStudentToParentAsync(model.ParentId, model.SelectedStudentId);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            IEnumerable<StudentDto> studentDtos = await studentService.GetStudentsNotAssignedToParentAsync(model.ParentId);
            model.AvailableStudents = new SelectList(
                mapper.Map<IEnumerable<StudentViewModel>>(studentDtos),
                "Id", 
                "FullName");
            return this.View(model);
        }        
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RemoveStudent(Guid parentId, Guid studentId)
    {
        ParentDto? parent = await parentService.GetParentByIdAsync(parentId);
        if (parent == null || (parent.UserId != this.User.Id() && !this.User.IsInRole("Administrator")))
        {
            return this.Forbid();
        }
        
        await parentService.RemoveStudentFromParentAsync(parentId, studentId);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await parentService.DeleteParentAsync(id);
        }
        catch (DbUpdateException)
        {
            this.TempData["Error"] = "Грешка: Има ученици към този родител.";
            return this.RedirectToAction(nameof(Index));
        }

        return this.RedirectToAction(nameof(Index));
    }
}
