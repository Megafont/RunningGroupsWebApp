using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;

using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<RaceModel> Races { get; set; }
        public DbSet<ClubModel> Clubs { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
    }
}
