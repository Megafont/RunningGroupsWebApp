using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.ViewModels;

namespace RunningGroupsWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly ApplicationDbContext _DbContext;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext dbContext)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _DbContext = dbContext;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) 
                return View(loginViewModel);

            var user = await _UserManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                // We found the user, so check the password.
                var passwordCheck = await _UserManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    // The password is correct, so sign the user in.
                    var result = await _SignInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Races");
                    }
                }

                // He said using TempData very much is a "design smell", and that normally you'd have a boolean in loginViewModel, such as loginViewModel.PasswordIsCorrect.
                TempData["Error"] = "Wrong credentials. Please try again.";
                return View(loginViewModel);
            }

            // The user was not found.
            TempData["Error"] = "Wrong credentials. Please try again.";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);

            var user = await _UserManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };

            var newUserResponse = await _UserManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
                await _UserManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Races");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Races");
        }
    }
}
