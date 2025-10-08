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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _dbContext.Plans.ToList();
        }

        public Plan? GetById(int id)
        {
            return _dbContext.Plans.Find(id);
        }

        public int Add(Plan plan)
        {
            _dbContext.Plans.Add(plan);
            return _dbContext.SaveChanges();
        }

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var plan = GetById(id);
            if (plan != null)
                _dbContext.Plans.Remove(plan);

            return _dbContext.SaveChanges();
        }
    }
}
