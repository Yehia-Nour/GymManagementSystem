using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ISessionRepository SessionRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new ();
        int SaveChanges();
    }
}
