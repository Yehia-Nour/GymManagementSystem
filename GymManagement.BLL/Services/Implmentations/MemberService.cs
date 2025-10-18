using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Implmentations;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (!members.Any())
                return [];

            var memberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberViewModels;
        }

        public MemberWithDetailsViewModel? GetMemberDetials(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null)
                return null;

            var viewModel = _mapper.Map<MemberWithDetailsViewModel>(member);

            var activeMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == id && ms.Status == "Active").FirstOrDefault();
            if (activeMemberShip is not null)
            {
                viewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                var emailExists = IsEmailExists(createMember.Email);
                var phoneExists = IsPhoneExists(createMember.Phone);
                if (emailExists || phoneExists)
                    return false;

                var member = _mapper.Map<Member>(createMember);

                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public MemberToUpdaterViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null)
                return null;

            var mebmerViewModel = _mapper.Map<MemberToUpdaterViewModel>(member);

            return mebmerViewModel;
        }

        public bool UpdateMemberDetials(int id, MemberToUpdaterViewModel memberToUpdater)
        {
            try
            {
                var emailExists = IsEmailExists(memberToUpdater.Email);

                var phoneExists = IsPhoneExists(memberToUpdater.Phone);

                if (emailExists || phoneExists)
                    return false;

                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = memberRepo.GetById(id);
                if (member is null)
                    return false;

                _mapper.Map(memberToUpdater, member);

                memberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public bool DeleteMember(int id)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = memberRepo.GetById(id);
                if (member is null)
                    return false;

                var hasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(ms => ms.MemberId == id && ms.Session.StartDate > DateTime.Now).Any();
                if (hasActiveMemberSession)
                    return false;

                var memberShips = _unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == id);
                if (memberShips.Any())
                {
                    foreach (var membership in memberShips)
                        _unitOfWork.GetRepository<MemberShip>().Delete(membership);
                }

                memberRepo.Delete(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetials(int id)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
            if (memberHealthRecord == null)
                return null;

            var healthRecordViewModel = _mapper.Map<HealthRecordViewModel>(memberHealthRecord);

            return healthRecordViewModel;
        }


        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();

        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
    }
}
