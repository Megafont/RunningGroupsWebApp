using Microsoft.AspNetCore.Mvc;

using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.ViewModels;

namespace RunningGroupsWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _UserRepository;

        public UserController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }

        [HttpGet("users")] // This makes it so this controller shows up as "users" in the url rather than "user".
        public async Task<IActionResult> Index()
        {
            var users = await _UserRepository.GetAllUsersAsync();

            List<UserViewModel> result = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                };
                result.Add(userViewModel);
            }

            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _UserRepository.GetUserByIdAsync(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };

            return View(userDetailViewModel);
        }
    }
}
