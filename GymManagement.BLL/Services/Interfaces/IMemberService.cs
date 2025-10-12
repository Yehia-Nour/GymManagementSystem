using GymManagement.BLL.ViewModels.MemberViewModel;
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
        HealthRecordViewModel? GetMemberHealthRecordDetials(int id);
    }
}
