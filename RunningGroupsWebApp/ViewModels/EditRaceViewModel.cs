using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Models;

namespace RunningGroupsWebApp.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int AddressId { get; set; }
        public AddressModel Address { get; set; }
        public RaceCategories RaceCategory { get; set; }
}
}
