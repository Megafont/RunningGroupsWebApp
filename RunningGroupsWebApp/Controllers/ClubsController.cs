using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubsRepository _ClubsRepository;
        private readonly IPhotoService _PhotoService;

        public ClubsController(IClubsRepository clubsRepository, IPhotoService photoService)
        {
            _ClubsRepository = clubsRepository;
            _PhotoService = photoService;
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
            return View();
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

            var userClub = await _ClubsRepository.GetByIdAsyncNoTracking(id);

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
    }
}
