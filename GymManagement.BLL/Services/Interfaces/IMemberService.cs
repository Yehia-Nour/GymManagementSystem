using GymManagement.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync();
        Task<MemberWithDetailsViewModel?> GetMemberDetialsAsync(int id);
        Task<bool> CreateMemberAsync(CreateMemberViewModel member);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id);
        Task<bool> UpdateMemberDetialsAsync(int id, MemberToUpdateViewModel memberToUpdater);
        Task<bool> DeleteMemberAsync(int id);
        Task<HealthRecordViewModel?> GetMemberHealthRecordDetialsAsync(int id);
    }
}
