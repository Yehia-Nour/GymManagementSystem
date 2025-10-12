using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Implmentations;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.Repositories.UnitOfWork.Interfaces;
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

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any())
                return [];

            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });

            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetials(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null)
                return null;

            var viewModel = new MemberViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };

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

                var member = new Member
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        City = createMember.City,
                        Street = createMember.Street,
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note ?? ""
                    }
                };

                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public MembeToUpdaterViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null)
                return null;

            return new MembeToUpdaterViewModel
            {
                Photo = member.Photo,
                Name = member.Name,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
            };
        }

        public bool UpdateMemberDetials(int id, MembeToUpdaterViewModel membeToUpdater)
        {
            try
            {
                var emailExists = IsEmailExists(membeToUpdater.Email);

                var phoneExists = IsPhoneExists(membeToUpdater.Phone);

                if (emailExists || phoneExists)
                    return false;

                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = memberRepo.GetById(id);
                if (member is null)
                    return false;

                member.Email = membeToUpdater.Email;
                member.Phone = membeToUpdater.Phone;
                member.Address.BuildingNumber = membeToUpdater.BuildingNumber;
                member.Address.Street = membeToUpdater.Street;
                member.Address.City = membeToUpdater.City;
                member.UpdatedAt = DateTime.Now;

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

            var healthRecordViewModel = new HealthRecordViewModel
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note,
            };

            return healthRecordViewModel;
        }


        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();

        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
    }
}
