using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.Models;
using RunningGroupsWebApp.Interfaces;

namespace RunningGroupsWebApp.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public DashboardRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _DbContext = dbContext;
            _HttpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ClubModel>> GetAllUserClubsAsync()
        {
            var curUser = _HttpContextAccessor.HttpContext?.User;
            var userClubs = _DbContext.Clubs.Where(r => r.AppUser.Id == curUser.ToString());

            return userClubs.ToList();
        }

        public async Task<List<RaceModel>> GetAllUserRacesAsync()
        {
            var curUser = _HttpContextAccessor.HttpContext?.User;
            var userRaces = _DbContext.Races.Where(r => r.AppUser.Id == curUser.ToString());

            return userRaces.ToList();
        }
    }
}
