using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync()
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllQueryable().ToListAsync();
            if (!plans.Any())
                return [];

            var planViewModels = _mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return planViewModels;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int id)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);
            if (plan is null)
                return null;

            var planViewModel = _mapper.Map<PlanViewModel>(plan);

            return planViewModel;
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id);
            if (plan is null || !plan.IsActive || await HasActiveMemberShips(id))
                return null;

            var planViewModel = _mapper.Map<UpdatePlanViewModel>(plan);

            return planViewModel;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel updatePlan)
        {
            try
            {
                var planRepo = _unitOfWork.GetRepository<Plan>();
                var plan = await planRepo.GetByIdAsync(id);
                if (plan is null || await HasActiveMemberShips(id))
                    return false;

                _mapper.Map(updatePlan, plan);

                planRepo.Update(plan);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }

        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await planRepo.GetByIdAsync(id);
            if (plan is null || await HasActiveMemberShips(id))
                return false;

            plan.IsActive = plan.IsActive ? false : true;

            try
            {
                planRepo.Update(plan);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }


        private async Task<bool> HasActiveMemberShips(int id) => await _unitOfWork.GetRepository<MemberShip>()
            .GetAllQueryable(ms => ms.PlanId == id && ms.Status == "Active").AnyAsync();
    }
}
