using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(GymDbContext dbContext) : base(dbContext) { }

        public IEnumerable<Session> GetAllSessionsWithTrainersAndCategories()
        {
            return _dbContext.Sessions.Include(s => s.Trainer)
                                      .Include(s => s.Capacity)
                                      .ToList();
        }
        public Session? GetSessionByIdWithTrainerandCategory(int id)
        {
            return _dbContext.Sessions.Include(s => s.Trainer)
                                      .Include(s => s.Capacity)
                                      .FirstOrDefault(s => s.Id == id);
        }
        public int GetCountOfBookedSlots(int id)
            => _dbContext.MembersSessions.Count(ms => ms.SessionId == id);
    }
}
