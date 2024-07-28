using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubsRepository _ClubsRepository;

        public ClubsController(IClubsRepository clubsRepository)
        {
            _ClubsRepository = clubsRepository;
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
        public async Task<IActionResult> Create(ClubModel club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }

            _ClubsRepository.Add(club);
            return RedirectToAction(nameof(Index));
        }
    }
}
