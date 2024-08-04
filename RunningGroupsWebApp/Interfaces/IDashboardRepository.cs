using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<RaceModel>> GetAllUserRacesAsync();
        Task<List<ClubModel>> GetAllUserClubsAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByIdNoTrackingAsync(string id);
        bool Save();
        bool Update(AppUser user);
    }
}
