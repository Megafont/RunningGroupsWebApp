using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class RacesController : Controller
    {
        private readonly IRacesRepository _RacesRepository;

        public RacesController(IRacesRepository racesRepository)
        {
            _RacesRepository = racesRepository;
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
        public async Task<IActionResult> Create(RaceModel race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }

            _RacesRepository.Add(race);
            return RedirectToAction(nameof(Index));
        }
    }
}
