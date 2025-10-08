using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class SessionRepository : ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAll()
        {
            return _dbContext.Sessions.ToList();
        }

        public Session? GetById(int id)
        {
            return _dbContext.Sessions.Find(id);
        }

        public int Add(Session session)
        {
            _dbContext.Sessions.Add(session);
            return _dbContext.SaveChanges();
        }

        public int Update(Session session)
        {
            _dbContext.Sessions.Update(session);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                _dbContext.Sessions.Remove(entity);

            return _dbContext.SaveChanges();
        }
    }
}
