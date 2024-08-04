using RunningGroupsWebApp.Models;

namespace RunningGroupsWebApp.Interfaces
{
    public interface IRacesRepository
    {
        Task<IEnumerable<RaceModel>> GetAllAsync();

        Task<RaceModel> GetByIdAsync(int id);
        Task<RaceModel> GetByIdNoTrackingAsync(int id);
        Task<IEnumerable<RaceModel>> GetAllByCityAsync(string city);

        // CRUD commands
        bool Add(RaceModel club);
        bool Update(RaceModel club);
        bool Delete(RaceModel club);
        bool Save();
    }
}
