using GymManagement.BLL.ViewModels.MembershipViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync();
        Task<bool> CraeteMembershipAsync(CreateMembershipViewModel createMembership);
        Task<bool> DeleteMembershipAsync(int id);
        Task<IEnumerable<MemberSelectListViewModel>> GetAllMembersForDropDownAsync();
        Task<IEnumerable<PlanSelectViewModel>> GetAllPlansForDropDownAsync();
    }
}
