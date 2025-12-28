using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        Task<IEnumerable<MemberShip>> GetAllMembershipsWithPlansAndMembersAsync();
    }
}
