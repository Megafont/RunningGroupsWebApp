using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
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
    }
}
