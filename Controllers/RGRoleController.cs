using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RGPatients.Models;
using System.Data;

namespace RGPatients.Controllers
{
    
    public class RGRoleController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RGRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            ViewBag.Roles = roles;
            return View();
        }

        [Authorize(Roles = "administrators")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRole model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = roleManager.FindByNameAsync(model.RoleName);

                if (roleExists.Result != null)
                {
                    TempData["message"] = "Role with the same name " + model.RoleName + " is aleady exists.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    try
                    {
                        IdentityRole role = new IdentityRole { Name = model.RoleName };

                        var result = await roleManager.CreateAsync(role);

                        if (result.Succeeded)
                        {
                            TempData["message"] = "Role added: " + model.RoleName;
                            return RedirectToAction(nameof(Index));
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = ex.GetBaseException().Message.ToString();
                        ModelState.AddModelError("", ex.GetBaseException().Message.ToString());
                    }

                }
            }
            else
            {
                TempData["message"] = "Role name is required.";
            }
            return View("Index");
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                // Error Mesage
            }

            var model = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                // Error Mesage
            }

            var model = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> DeleteRoleConfirm(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                // Error Mesage
            }

            var model = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }


        [Authorize(Roles = "administrators")]
        [HttpPost]
        public async Task<IActionResult> Delete(EditRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                // Error Mesage
            }
            else
            {
                try
                {

                    var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);


                    role.Name = model.RoleName;

                    TempData["message"] = "Role deleted: " + role.Name;
                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                catch (Exception ex)
                {
                    TempData["message"] = ex.GetBaseException().Message.ToString();
                    ModelState.AddModelError("", ex.GetBaseException().Message.ToString());
                }
            }

            return View(model);
        }


        [Authorize(Roles = "administrators")]
        [HttpPost]
        public async Task<IActionResult> DeleteRoleConfirm(EditRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                // Error Mesage
            }
            else
            {
                try
                {
                    role.Name = model.RoleName;
                    var result = await roleManager.DeleteAsync(role);
                    TempData["message"] = "Role deleted: " + model.RoleName;

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {
                    TempData["message"] = ex.GetBaseException().Message.ToString();
                    ModelState.AddModelError("", ex.GetBaseException().Message.ToString());
                }
            }

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> UsersInRole(string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            ViewBag.RoleName = role.Name;
            ViewBag.RoleId = RoleId;

            var model = new List<UserRole>();
            var usersNotInRole = new List<UserRole>();

            foreach (var user in userManager.Users)
            {

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    var userrole = new UserRole
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };


                    model.Add(userrole);
                }
                else
                {
                    var userrole = new UserRole
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    usersNotInRole.Add(userrole);
                }
            }

            ViewData["Users"] = new SelectList(usersNotInRole.OrderBy(x => x.UserName), "UserId", "UserName");

            ViewBag.UsersInRole = model;

            return View();
        }


        [Authorize(Roles = "administrators")]
        [HttpPost]
        public async Task<IActionResult> AddUserInRole(UserRole model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                // Error meesage
                ViewBag.ErrorMessage = $"Role with id = {model.RoleId} cannot be found";
                TempData["message"] = $"Role with id = {model.RoleId} cannot be found";
            }

            try
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                IdentityResult result = null;
                if (!(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                    TempData["message"] = "User " + user.UserName + " added in role: " + role.Name;
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.GetBaseException().Message.ToString();
                ModelState.AddModelError("", ex.GetBaseException().Message.ToString());
            }
            return RedirectToAction("UsersInRole", new { RoleId = model.RoleId });
        }


        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Error meesage
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
            }

            var model = new List<UserRole>();

            foreach (var user in userManager.Users)
            {
                var userrole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                    userrole.IsSelected = true;
                else
                    userrole.IsSelected = false;
                model.Add(userrole);
            }

            return View(model);
        }


        [Authorize(Roles = "administrators")]
        [HttpGet]
        public async Task<IActionResult> Remove(string RoleId, string UserId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                // Error meesage
                ViewBag.ErrorMessage = $"Role with id = {RoleId} cannot be found";
                TempData["message"] = $"Role with id = {RoleId} cannot be found";
            }
            else
            {
                try
                {
                    var user = await userManager.FindByIdAsync(UserId);
                    IdentityResult result = null;

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                        TempData["message"] = "User " + user.UserName + " removed from role " + role.Name;
                    }
                }
                catch (Exception ex)
                {
                    TempData["message"] = ex.GetBaseException().Message.ToString();
                    ModelState.AddModelError("", ex.GetBaseException().Message.ToString());
                }
            }

            return RedirectToAction("UsersInRole", new { RoleId = RoleId });
        }

    }
}
