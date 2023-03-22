using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.Models;
using ShoppingCart.Logic.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }



        [HttpGet]
        public IActionResult Users()
        {
            return View(userManager.Users);
        }


        [HttpGet]
        public IActionResult Roles()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]
        public IActionResult CreateRoles()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoles([MinLength(2), Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    TempData["Success"] = "The role has been created!";
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ModelState.AddModelError("", "Minimum length is 2");
            return View(name);
        }



        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<ApplicationUser> members = new List<ApplicationUser>();

            List<ApplicationUser> nonMembers = new List<ApplicationUser>();

            foreach (var user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEditViewModel
            {

                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(RoleEditViewModel roleEdit)
        {
            IdentityResult result;

            foreach (string userId in roleEdit.AddIds ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);

                result = await userManager.AddToRoleAsync(user, roleEdit.RoleName);
            }

            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);

                result = await userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
