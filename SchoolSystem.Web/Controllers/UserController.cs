using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Common;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.User;

namespace SchoolSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class UserController(IUserService userService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<UserDto> users = await userService.GetUsersAsync();
        IEnumerable<UserViewModel> usersVm = mapper.Map<IEnumerable<UserViewModel>>(users);
        return this.View(usersVm);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        UserDto? user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return this.NotFound();
        }
        
        UserEditViewModel userVm = mapper.Map<UserEditViewModel>(user);
        return this.View(userVm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await userService.EditUserAsync(mapper.Map<UserDto>(model));
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        UserDto? user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return this.NotFound();
        }

        UserViewModel userVm = mapper.Map<UserViewModel>(user);
        return this.View(userVm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
