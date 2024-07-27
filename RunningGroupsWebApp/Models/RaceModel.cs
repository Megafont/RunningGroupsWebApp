using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using RunGroupWebApp.Data.Enums;


namespace RunGroupWebApp.Models
{
    public class RaceModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public AddressModel Address { get; set; }
        public RaceCategories RaceCategory { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
