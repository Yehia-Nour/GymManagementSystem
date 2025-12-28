using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<MemberShip>> GetAllMembershipsWithPlansAndMembersAsync()
        {
            return await _dbContext.MembersShips
                 .Where(ms => ms.EndDate >= DateTime.Now)
                 .Include(ms => ms.Plan)
                 .Include(ms => ms.Member)
                 .AsNoTracking()
                 .ToListAsync();
        }
    }
}
