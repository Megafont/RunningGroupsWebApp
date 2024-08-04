using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Models;
using RunningGroupsWebApp.ViewModels;

namespace RunningGroupsWebApp.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubsRepository _ClubsRepository;
        private readonly IPhotoService _PhotoService;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public ClubsController(IClubsRepository clubsRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _ClubsRepository = clubsRepository;
            _PhotoService = photoService;
            _HttpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ClubModel> clubs = await _ClubsRepository.GetAllAsync();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // NOTE: Use Include() sparingly as it is a much more expensive database operation.
            ClubModel club = await _ClubsRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _HttpContextAccessor.HttpContext.User.GetUserId(); // GetUserId() is an extension method we made in ClaimPrincipalExtensions.cs.

            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };

            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _PhotoService.AddPhotoAsync(clubVM.Image);

                var club = new ClubModel
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new AddressModel
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    },
                };

                _ClubsRepository.Add(club);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed!");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _ClubsRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                ClubCategory = club.ClubCategory,
                URL = club.Image
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club!");
                return View("Edit", clubVM);
            }

            var userClub = await _ClubsRepository.GetByIdNoTrackingAsync(id);

            if (userClub != null)
            {
                try
                {
                    var fi = new FileInfo(userClub.Image);
                    var publicId = Path.GetFileNameWithoutExtension(fi.Name);
                    await _PhotoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo!");
                    return View(clubVM);
                }


                var photoResult = await _PhotoService.AddPhotoAsync(clubVM.Image);
                var club = new ClubModel()
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AddressId = userClub.AddressId,
                    Address = new AddressModel
                    {
                        Id = userClub.AddressId,
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    },
                    ClubCategory = clubVM.ClubCategory,
                    Image = photoResult.Url.ToString(),
                };

                _ClubsRepository.Update(club);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _ClubsRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _ClubsRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            // Delete the photo
            try
            {
                var fi = new FileInfo(clubDetails.Image);
                var publicId = Path.GetFileNameWithoutExtension(fi.Name);
                await _PhotoService.DeletePhotoAsync(publicId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not delete photo!");
                return View(clubDetails);
            }

            // Delete the club
            _ClubsRepository.Delete(clubDetails);
            return RedirectToAction(nameof(Index));
        }
      
    }
}
