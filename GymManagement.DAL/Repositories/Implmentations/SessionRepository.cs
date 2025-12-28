using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(GymDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainersAndCategoriesAsync()
        {
            return await _dbContext.Sessions.Include(s => s.Trainer)
                                      .Include(s => s.Category)
                                      .AsNoTracking()
                                      .ToListAsync();
        }
        public async Task<Session?> GetSessionByIdWithTrainerandCategoryAsync(int id)
        {
            return await _dbContext.Sessions.Include(s => s.Trainer)
                                      .Include(s => s.Category)
                                      .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<int> GetCountOfBookedSlotsAsync(int id)
            => await _dbContext.MembersSessions.CountAsync(ms => ms.SessionId == id);
    }
}
