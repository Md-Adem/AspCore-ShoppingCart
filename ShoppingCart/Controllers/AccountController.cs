using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using ShoppingCart.Data.Models;
using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private IPasswordHasher<ApplicationUser> passwordHasher;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser AppUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

                var result = await userManager.CreateAsync(AppUser, user.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel login = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(login);
        }


        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser AppUser = await userManager.FindByEmailAsync(login.Email);

                if (AppUser != null)
                {
                    var result = await signInManager.PasswordSignInAsync(AppUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");
                    }

                    ModelState.AddModelError("", "Login failed");
                }
            }

            return View(login);
        }



        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Redirect("/");
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ApplicationUser AppUser = await userManager.FindByNameAsync(User.Identity.Name);

            UserViewModel user = new UserViewModel(AppUser);

            return View(user);
        }



        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            ApplicationUser AppUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                if (user.Email != null)
                {
                    AppUser.Email = user.Email;
                }

                if (user.UserName != null)
                {
                    AppUser.UserName = user.UserName;
                }

                if (user.Password != null)
                {
                    AppUser.PasswordHash = passwordHasher.HashPassword(AppUser, user.Password);
                }

                var result = await userManager.UpdateAsync(AppUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "You information has been updated!";
                }
            }

            return View();
        }
    }
}
