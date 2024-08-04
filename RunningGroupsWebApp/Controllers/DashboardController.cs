using Microsoft.AspNetCore.Mvc;

using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.ViewModels;

namespace RunningGroupsWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _DashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _DashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userClubs = await _DashboardRepository.GetAllUserClubsAsync();
            var userRaces = await _DashboardRepository.GetAllUserRacesAsync();

            var dashboardViewModel = new DashboardViewModel()
            {
                Clubs = userClubs,
                Races = userRaces,
            };

            return View(dashboardViewModel);
        }
    }
}
