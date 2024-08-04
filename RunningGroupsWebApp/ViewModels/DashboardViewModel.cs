using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<ClubModel> Clubs { get; set; }
        public List<RaceModel> Races { get; set; }
    }
}
