using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetAllMembershipsWithPlansAndMembers();
    }
}
