using GymManagement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        MemberWithDetailsViewModel? GetMemberDetials(int id);
        bool CreateMember(CreateMemberViewModel member);
        MemberToUpdateViewModel? GetMemberToUpdate(int id);
        bool UpdateMemberDetials(int id, MemberToUpdateViewModel memberToUpdater);
        bool DeleteMember(int id);
        HealthRecordViewModel? GetMemberHealthRecordDetials(int id);
    }
}
