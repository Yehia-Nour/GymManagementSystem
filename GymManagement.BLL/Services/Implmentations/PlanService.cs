using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (!plans.Any())
                return [];

            var planViewModels = _mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return planViewModels;
        }

        public PlanViewModel? GetPlanDetails(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null)
                return null;

            var planViewModel = _mapper.Map<PlanViewModel>(plan);

            return planViewModel;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null || !plan.IsActive || HasActiveMemberShips(id))
                return null;

            var planViewModel = _mapper.Map<UpdatePlanViewModel>(plan);

            return planViewModel;
        }

        public bool UpdatePlan(int id, UpdatePlanViewModel updatePlan)
        {
            try
            {
                var planRepo = _unitOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(id);
                if (plan is null || HasActiveMemberShips(id))
                    return false;

                _mapper.Map(updatePlan, plan);

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
