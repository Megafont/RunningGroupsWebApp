using Microsoft.EntityFrameworkCore;
using RunningGroupsWebApp.Data;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _DbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _DbContext.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _DbContext.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _DbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _DbContext.Update(user);
            return Save();
        }
    }
}
