using GymManagement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlantService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int id);
        UpdatePlanViewModel? GetPlanToUpdate(int id);
        bool UpdatePlan (int id, UpdatePlanViewModel updatePlan); 
        bool ToggleStatus (int id);
    }
}
