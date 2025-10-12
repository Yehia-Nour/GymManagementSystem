using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModel;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Implmentations;
using GymManagement.DAL.Repositories.Interfaces;
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
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<MemberSession> _memberSessionRepository;

        public MemberService(IGenericRepository<Member> memberRepository,
            IGenericRepository<MemberShip> emberShipRepository,
            IPlanRepository planRepository,
            IGenericRepository<HealthRecord> healthRecordRepository,
            IGenericRepository<MemberSession> memberSessionRepository)
        {
            _memberRepository = memberRepository;
            _memberShipRepository = emberShipRepository;
            _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
            _memberSessionRepository = memberSessionRepository;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _memberRepository.GetAll();
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
            var member = _memberRepository.GetById(id);
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

            var activeMemberShip = _memberShipRepository.GetAll(ms => ms.MemberId == id && ms.Status == "Active").FirstOrDefault();
            if (activeMemberShip is not null)
            {
                viewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();
                var plan = _planRepository.GetById(activeMemberShip.PlanId);
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

                return _memberRepository.Add(member) > 0;
            }
            catch
            {
                return false;
            }
        }

        public MembeToUpdaterViewModel? GetMemberToUpdate(int id)
        {
            var member = _memberRepository.GetById(id);
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

                var member = _memberRepository.GetById(id);
                if (member is null)
                    return false;

                member.Email = membeToUpdater.Email;
                member.Phone = membeToUpdater.Phone;
                member.Address.BuildingNumber = membeToUpdater.BuildingNumber;
                member.Address.Street = membeToUpdater.Street;
                member.Address.City = membeToUpdater.City;
                member.UpdatedAt = DateTime.Now;

                return _memberRepository.Update(member) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMember(int id)
        {
            try
            {
                var member = _memberRepository.GetById(id);
                if (member is null)
                    return false;

                var hasActiveMemberSession = _memberSessionRepository
                    .GetAll(ms => ms.MemberId == id && ms.Session.StartDate > DateTime.Now).Any();
                if (hasActiveMemberSession)
                    return false;

                var memberShips = _memberShipRepository.GetAll(ms => ms.MemberId == id);
                if (memberShips.Any())
                {
                    foreach (var membership in memberShips)
                        _memberShipRepository.Delete(membership);
                }

                return _memberRepository.Delete(member) > 0;
            }
            catch
            {
                return false;
            }
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetials(int id)
        {
            var memberHealthRecord = _healthRecordRepository.GetById(id);
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


        private bool IsEmailExists(string email) => _memberRepository.GetAll(m => m.Email == email).Any();

        private bool IsPhoneExists(string phone) => _memberRepository.GetAll(m => m.Phone == phone).Any();
    }
}
