using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.BLL.Services.Implmentations
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync()
        {
            var memberships = await _unitOfWork.MembershipRepository.GetAllMembershipsWithPlansAndMembersAsync();
            if (!memberships.Any())
                return [];

            var membershipViewModels = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return membershipViewModels;
        }

        public async Task<bool> CraeteMembershipAsync(CreateMembershipViewModel createMembership)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(createMembership.MemberId);
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(createMembership.PlanId);
            if (member is null || plan is null || !plan.IsActive)
                return false;

            var repo = _unitOfWork.MembershipRepository;
            var MemberHasActiveMembership = await repo.GetAllQueryable(ms => ms.MemberId == createMembership.MemberId && ms.EndDate >= DateTime.Now).AnyAsync();
            if (MemberHasActiveMembership)
                return false;

            var membership = _mapper.Map<MemberShip>(createMembership);
            membership.EndDate = DateTime.Now.AddDays(plan.DurationDays);

            await repo.AddAsync(membership);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMembershipAsync(int id)
        {
            var membership = await _unitOfWork.GetRepository<MemberShip>().GetByIdAsync(id);

            if (membership is null || membership.Status == "Expired")
                return false;

            _unitOfWork.GetRepository<MemberShip>().Delete(membership);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetAllMembersForDropDownAsync()
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllQueryable().ToListAsync();

            var memberSelectViewModels = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);

            return memberSelectViewModels;
        }

        public async Task<IEnumerable<PlanSelectViewModel>> GetAllPlansForDropDownAsync()
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllQueryable().ToListAsync();

            var planSelectViewModels = _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);

            return planSelectViewModels;
        }
    }
}
