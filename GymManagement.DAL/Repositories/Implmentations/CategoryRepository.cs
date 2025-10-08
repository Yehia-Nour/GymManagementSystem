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
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;

        public HealthRecordRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<HealthRecord> GetAll()
        {
            return _dbContext.HealthRecords.ToList();
        }

        public HealthRecord? GetById(int id)
        {
            return _dbContext.HealthRecords.Find(id);
        }

        public int Add(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Add(healthRecord);
            return _dbContext.SaveChanges();
        }

        public int Update(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Update(healthRecord);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                _dbContext.HealthRecords.Remove(entity);

            return _dbContext.SaveChanges();
        }
    }
}
