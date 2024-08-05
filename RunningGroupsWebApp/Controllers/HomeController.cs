using System.Diagnostics;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using RunningGroupsWebApp.Helpers;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Models;
using RunningGroupsWebApp.ViewModels;


namespace RunningGroupsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubsRepository _ClubRepository;

        public HomeController(ILogger<HomeController> logger, IClubsRepository clubRepository)
        {
            _logger = logger;
            _ClubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();

            try
            {
                string url = "https://ipinfo.io?token=52087235a7035d";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                
                ipInfo.Country = myRI1.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;

                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _ClubRepository.GetAllByCityAsync(homeViewModel.City);
                }
                else
                {
                    homeViewModel.Clubs = null;
                }

                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
