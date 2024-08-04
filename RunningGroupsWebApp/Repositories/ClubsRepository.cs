using Microsoft.EntityFrameworkCore;
using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.Repositories
{
    public class ClubsRepository : IClubsRepository
    {
        private readonly ApplicationDbContext _DbContext;

        public ClubsRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public bool Add(ClubModel club)
        {
            _DbContext.Add(club);
            return Save();
        }

        public bool Delete(ClubModel club)
        {
            _DbContext.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<ClubModel>> GetAllAsync()
        {
            return await _DbContext.Clubs.ToListAsync();
        }

        public async Task<ClubModel> GetByIdAsync(int id)
        {
            // The Include() call tells it to also get the address data when it fetches the requested club from the database table.
            // This is because the Address is in another table, and it will not look it up if you don't use Include().
            return await _DbContext.Clubs.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ClubModel> GetByIdNoTrackingAsync(int id)
        {
            // The Include() call tells it to also get the address data when it fetches the requested club from the database table.
            // This is because the Address is in another table, and it will not look it up if you don't use Include().
            return await _DbContext.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ClubModel>> GetAllByCityAsync(string city)
        {
            return await _DbContext.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _DbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ClubModel club)
        {
            _DbContext.Update(club);
            return Save();
        }
    }
}
