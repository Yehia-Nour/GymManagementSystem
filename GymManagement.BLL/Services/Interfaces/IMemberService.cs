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
        bool CreateMember(CreateMemberViewModel member);
        MemberViewModel? GetMemberDetials(int id);
        MembeToUpdaterViewModel? GetMemberToUpdate(int id);
        bool UpdateMemberDetials(int id, MembeToUpdaterViewModel membeToUpdater);
        bool DeleteMember(int id);
        HealthRecordViewModel? GetMemberHealthRecordDetials(int id);
    }
}
