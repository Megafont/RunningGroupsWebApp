using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<RaceModel>> GetAllUserRacesAsync();
        Task<List<ClubModel>> GetAllUserClubsAsync();
    }
}
