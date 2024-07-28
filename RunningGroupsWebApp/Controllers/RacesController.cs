using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RacesController : Controller
    {
        private readonly IRacesRepository _RacesRepository;
        private readonly IPhotoService _PhotoService;

        public RacesController(IRacesRepository racesRepository, IPhotoService photoService)
        {
            _RacesRepository = racesRepository;
            _PhotoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var races = await _RacesRepository.GetAllAsync();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // NOTE: Use Include() sparingly as it is a much more expensive database operation.
            RaceModel race = await _RacesRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _PhotoService.AddPhotoAsync(raceVM.Image);

                var race = new RaceModel
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new AddressModel
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State
                    },
                };

                _RacesRepository.Add(race);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed!");
            }

            return View(raceVM);
        }

    }
}
