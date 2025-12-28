using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainersAndCategoriesAsync();
        Task<Session?> GetSessionByIdWithTrainerandCategoryAsync(int id);
        Task<int> GetCountOfBookedSlotsAsync(int id);
    }
}
