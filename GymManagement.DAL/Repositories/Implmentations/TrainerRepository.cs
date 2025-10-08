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
    public class TrainerRepository : ITrainerRepository
    {
        private readonly GymDbContext _dbContext;

        public TrainerRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Trainer> GetAll()
        {
            return _dbContext.Trainers.ToList();
        }

        public Trainer? GetById(int id)
        {
            return _dbContext.Trainers.Find(id);
        }

        public int Add(Trainer trainer)
        {
            _dbContext.Trainers.Add(trainer);
            return _dbContext.SaveChanges();
        }

        public int Update(Trainer trainer)
        {
            _dbContext.Trainers.Update(trainer);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                _dbContext.Trainers.Remove(entity);

            return _dbContext.SaveChanges();
        }
    }
}
