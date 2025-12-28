using GymManagement.BLL.ViewModels.PlanViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync();
        Task<PlanViewModel?> GetPlanDetailsAsync(int id);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id);
        Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel updatePlan);
        Task<bool> ToggleStatusAsync(int id);
    }
}
