using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<MemberSession>
    {
        Task<IEnumerable<MemberSession>> GetSessionByIdAsync(int id);
    }
}
