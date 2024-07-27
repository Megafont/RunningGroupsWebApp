using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IClubsRepository
    {
        Task<IEnumerable<ClubModel>> GetAllAsync();

        Task<ClubModel> GetByIdAsync(int id);
        Task<IEnumerable<ClubModel>> GetAllByCityAsync(string city);

        // CRUD commands
        bool Add(ClubModel club);
        bool Update(ClubModel club);
        bool Delete(ClubModel club);
        bool Save();
    }
}
