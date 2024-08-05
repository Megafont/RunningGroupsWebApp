using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ClubModel> Clubs { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
