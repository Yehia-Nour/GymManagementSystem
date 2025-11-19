using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainersAndCategoriesAsync();
        Task<Session?> GetSessionByIdWithTrainerandCategoryAsync(int id);
        Task<int> GetCountOfBookedSlotsAsync(int id);
    }
}
