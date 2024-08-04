using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.Models;
using RunningGroupsWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var curUser = _HttpContextAccessor.HttpContext?.User.GetUserId(); // GetUserId() is an extension method we made in ClaimPrincipalExtensions.cs.
            var userClubs = _DbContext.Clubs.Where(r => r.AppUser.Id == curUser);

            return userClubs.ToList();
        }

        public async Task<List<RaceModel>> GetAllUserRacesAsync()
        {
            var curUser = _HttpContextAccessor.HttpContext?.User.GetUserId(); // GetUserId() is an extension method we made in ClaimPrincipalExtensions.cs.
            var userRaces = _DbContext.Races.Where(r => r.AppUser.Id == curUser);

            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _DbContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdNoTrackingAsync(string id)
        {
            return await _DbContext.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _DbContext.SaveChanges();
            return saved > 0 ? true: false;
        }

        public bool Update(AppUser user)
        {
            _DbContext.Users.Update(user);
            return Save();
        }
    }
}
