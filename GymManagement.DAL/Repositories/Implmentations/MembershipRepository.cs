using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext dbContext) : base(dbContext) { }
        public IEnumerable<MemberShip> GetAllMembershipsWithPlansAndMembers()
        {
            return _dbContext.MembersShips
                 .Include(ms => ms.Plan)
                 .Include(ms => ms.Member)
                 .AsNoTracking()
                 .ToList();
        }
    }
}
