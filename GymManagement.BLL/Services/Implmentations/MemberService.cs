using AutoMapper;
using GymManagement.BLL.Services.AttachmentService;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Implmentations;
using GymManagement.DAL.Repositories.Interfaces;
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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync()
        {
            var members =  await _unitOfWork.GetRepository<Member>().GetAllQueryable().ToListAsync();
            if (!members.Any())
                return [];

            var memberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberViewModels;
        }

        public async Task<MemberWithDetailsViewModel?> GetMemberDetialsAsync(int id)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member is null)
                return null;

            var viewModel = _mapper.Map<MemberWithDetailsViewModel>(member);

            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().GetAllQueryable(ms => ms.MemberId == id && ms.Status == "Active").FirstOrDefaultAsync();
            if (activeMemberShip is not null)
            {
                viewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMemberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMember)
        {
            try
            {
                var emailExists = await IsEmailExistsAsync(createMember.Email);
                var phoneExists = await IsPhoneExistsAsync(createMember.Phone);
                if (emailExists || phoneExists)
                    return false;

                var photoName = _attachmentService.Upload("Members", createMember.Photo);
                if (string.IsNullOrEmpty(photoName))
                    return false;



                var member = _mapper.Map<Member>(createMember);
                member.Photo = photoName;

                await _unitOfWork.GetRepository<Member>().AddAsync(member);
                var isCreated = await _unitOfWork.SaveChangesAsync() > 0;
                if (!isCreated)
                    _attachmentService.Delete(photoName, "Members");

                return isCreated;
            }
            catch { return false; }
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member is null)
                return null;

            var mebmerViewModel = _mapper.Map<MemberToUpdateViewModel>(member);

            return mebmerViewModel;
        }

        public async Task<bool> UpdateMemberDetialsAsync(int id, MemberToUpdateViewModel memberToUpdater)
        {
            try
            {
                var emailExists = await _unitOfWork.GetRepository<Member>()
                    .GetAllQueryable(m => m.Email == memberToUpdater.Email && m.Id != id).AnyAsync();

                var phoneExists = await _unitOfWork.GetRepository<Member>()
                    .GetAllQueryable(m => m.Phone == memberToUpdater.Phone && m.Id != id).AnyAsync();

                if (emailExists || phoneExists)
                    return false;

                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = await memberRepo.GetByIdAsync(id);
                if (member is null)
                    return false;

                _mapper.Map(memberToUpdater, member);

                memberRepo.Update(member);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = await memberRepo.GetByIdAsync(id);
                if (member is null)
                    return false;

                var sessionIds = await _unitOfWork.GetRepository<MemberSession>()
                    .GetAllQueryable(ms => ms.MemberId == id).Select(ms => ms.SessionId).ToListAsync();

                var hasActiveMemberSession =  await _unitOfWork.GetRepository<Session>()
                    .GetAllQueryable(s => sessionIds.Contains(s.Id) && s.StartDate > DateTime.Now).AnyAsync();
                if (hasActiveMemberSession)
                    return false;

                var memberShips = _unitOfWork.GetRepository<MemberShip>().GetAllQueryable(ms => ms.MemberId == id);
                if (memberShips.Any())
                {
                    foreach (var membership in memberShips)
                        _unitOfWork.GetRepository<MemberShip>().Delete(membership);
                }

                memberRepo.Delete(member);
                var isDeleted = await _unitOfWork.SaveChangesAsync() > 0;
                if (isDeleted)
                    _attachmentService.Delete(member.Photo, "Members");

                return isDeleted;
            }
            catch { return false; }
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordDetialsAsync(int id)
        {
            var memberHealthRecord = await _unitOfWork.GetRepository<HealthRecord>().GetByIdAsync(id);
            if (memberHealthRecord == null)
                return null;

            var healthRecordViewModel = _mapper.Map<HealthRecordViewModel>(memberHealthRecord);

            return healthRecordViewModel;
        }


        private async Task<bool> IsEmailExistsAsync(string email) => await _unitOfWork.GetRepository<Member>().GetAllQueryable(m => m.Email == email).AnyAsync();

        private async Task<bool> IsPhoneExistsAsync(string phone) => await _unitOfWork.GetRepository<Member>().GetAllQueryable(m => m.Phone == phone).AnyAsync();
    }
}
