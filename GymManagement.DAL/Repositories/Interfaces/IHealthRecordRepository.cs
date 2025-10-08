using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetAll();
        HealthRecord? GetById(int id);
        int Add(HealthRecord healthRecord);
        int Update(HealthRecord healthRecord);
        int Delete(int id);
    }
}
