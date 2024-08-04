using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Models;
using RunningGroupsWebApp.ViewModels;

namespace RunningGroupsWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _DashboardRepository;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IPhotoService _PhotoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _DashboardRepository = dashboardRepository;
            _HttpContextAccessor = httpContextAccessor;
            _PhotoService = photoService;
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.State = editVM.State;
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


        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _HttpContextAccessor.HttpContext.User.GetUserId();
            var user = await _DashboardRepository.GetUserByIdAsync(curUserId);

            if (user == null)
                return View("Error");

            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile.");
                return View("EditUserProfile", editVM);
            }

            var user = await _DashboardRepository.GetUserByIdNoTrackingAsync(editVM.Id);
            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var photoResult = await _PhotoService.AddPhotoAsync(editVM.Image);
                
                MapUserEdit(user, editVM, photoResult);

                _DashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _PhotoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the previous photo.");
                    return View(editVM);
                }

                var photoResult = await _PhotoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _DashboardRepository.Update(user);

                return RedirectToAction("Index");
            }

        }


    }
}
