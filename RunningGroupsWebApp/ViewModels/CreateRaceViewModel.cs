using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Models;

namespace RunningGroupsWebApp.ViewModels
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AddressModel Address { get; set; }
        public IFormFile Image { get; set; }
        public RaceCategories RaceCategory { get; set; }
    }
}
