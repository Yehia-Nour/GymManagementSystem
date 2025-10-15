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
        MemberToUpdaterViewModel? GetMemberToUpdate(int id);
        bool UpdateMemberDetials(int id, MemberToUpdaterViewModel memberToUpdater);
        bool DeleteMember(int id);
        HealthRecordViewModel? GetMemberHealthRecordDetials(int id);
    }
}
