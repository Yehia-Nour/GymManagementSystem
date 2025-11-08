using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
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
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllMembershipsWithPlansAndMembers();
            if (!memberships.Any())
                return [];

            var membershipViewModels = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return membershipViewModels;
        }

        public bool CraeteMembership(CreateMembershipViewModel createMembership)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(createMembership.MemberId);
            var plan = _unitOfWork.GetRepository<Plan>().GetById(createMembership.PlanId);
            if (member is null || plan is null || !plan.IsActive)
                return false;

            var repo = _unitOfWork.MembershipRepository;
            var MemberHasActiveMembership = repo.GetAll(ms => ms.MemberId == createMembership.MemberId && ms.Status == "Active").Any();
            if (MemberHasActiveMembership)
                return false;

            var membership = _mapper.Map<MemberShip>(createMembership);
            membership.EndDate = DateTime.Now.AddDays(plan.DurationDays);

            repo.Add(membership);

            return _unitOfWork.SaveChanges() > 0;
        }

        public bool DeleteMembership(int id)
        {
            var membership = _unitOfWork.GetRepository<MemberShip>().GetById(id);

            if (membership is null || membership.Status == "Expired") 
                return false;

            _unitOfWork.GetRepository<MemberShip>().Delete(membership);

            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MemberSelectViewModel> GetAllMembersForDropDown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

            var memberSelectViewModels = _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);

            return memberSelectViewModels;
        }

        public IEnumerable<PlanSelectViewModel> GetAllPlansForDropDown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();

            var planSelectViewModels = _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);

            return planSelectViewModels;
        }
    }
}
