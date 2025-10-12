using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class PlantService : IPlantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any())
                return [];

            var planViewModels = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });

            return planViewModels;
        }

        public PlanViewModel? GetPlanDetails(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null)
                return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null || !plan.IsActive || HasActiveMemberShips(id))
                return null;

            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }

        public bool UpdatePlan(int id, UpdatePlanViewModel updatePlan)
        {
            try
            {
                var planRepo = _unitOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(id);
                if (plan is null || HasActiveMemberShips(id))
                    return false;

                plan.Description = updatePlan.Description;
                plan.DurationDays = updatePlan.DurationDays;
                plan.Price = updatePlan.Price;
                plan.UpdatedAt = DateTime.Now;

                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }

        }

        public bool ToggleStatus(int id)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = planRepo.GetById(id);
            if (plan is null || HasActiveMemberShips(id))
                return false;

            plan.IsActive = plan.IsActive ? false : true;

            try
            {
                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }


        private bool HasActiveMemberShips(int id) => _unitOfWork.GetRepository<MemberShip>()
            .GetAll(ms => ms.PlanId == id && ms.Status == "Active").Any();
    }
}
