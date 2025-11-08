using GymManagement.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();
        bool CraeteMembership(CreateMembershipViewModel createMembership);
        bool DeleteMembership(int id);
        IEnumerable<MemberSelectViewModel> GetAllMembersForDropDown();
        IEnumerable<PlanSelectViewModel> GetAllPlansForDropDown();
    }
}
