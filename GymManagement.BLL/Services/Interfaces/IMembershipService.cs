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
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync();
        Task<bool> CraeteMembershipAsync(CreateMembershipViewModel createMembership);
        Task<bool> DeleteMembershipAsync(int id);
        Task<IEnumerable<MemberSelectViewModel>> GetAllMembersForDropDownAsync();
        Task<IEnumerable<PlanSelectViewModel>> GetAllPlansForDropDownAsync();
    }
}
