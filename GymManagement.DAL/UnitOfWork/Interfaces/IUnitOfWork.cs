using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ISessionRepository SessionRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IBookingRepository BookingRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync();
    }
}
