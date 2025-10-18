using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class MemberWithDetailsViewModel : MemberViewModel
    {
        public string PlanName { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string MemberShipStartDate { get; set; } = null!;
        public string MemberShipEndDate { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
