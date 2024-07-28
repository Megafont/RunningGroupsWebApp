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
    }
}
