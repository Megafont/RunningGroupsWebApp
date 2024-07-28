using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class RacesRepository : IRacesRepository
    {
        private readonly ApplicationDbContext _DbContext;

        public RacesRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public bool Add(RaceModel race)
        {
            _DbContext.Add(race);
            return Save();
        }

        public bool Delete(RaceModel race)
        {
            _DbContext.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<RaceModel>> GetAllAsync()
        {
            return await _DbContext.Races.ToListAsync();
        }

        public async Task<RaceModel> GetByIdAsync(int id)
        {
            // The Include() call tells it to also get the address data when it fetches the requested club from the database table.
            // This is because the Address is in another table, and it will not look it up if you don't use Include().
            return await _DbContext.Races.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<RaceModel> GetByIdAsyncNoTracking(int id)
        {
            // The Include() call tells it to also get the address data when it fetches the requested club from the database table.
            // This is because the Address is in another table, and it will not look it up if you don't use Include().
            return await _DbContext.Races.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<RaceModel>> GetAllByCityAsync(string city)
        {
            return await _DbContext.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _DbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(RaceModel race)
        {
            _DbContext.Update(race);
            return Save();
        }
    }
}
