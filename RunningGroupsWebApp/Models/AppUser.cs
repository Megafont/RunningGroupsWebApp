using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;


namespace RunningGroupsWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public AddressModel? Address { get; set; }
        public ICollection<ClubModel> Clubs { get; set; }
        public ICollection<RaceModel> Races { get; set; }
    }
}

