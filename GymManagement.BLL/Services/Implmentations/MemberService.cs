using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModel;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Implmentations;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _emberShipRepository;
        private readonly IPlanRepository _planRepository;

        public MemberService(IGenericRepository<Member> memberRepository,
            IGenericRepository<MemberShip> emberShipRepository,
            IPlanRepository planRepository)
        {
            _memberRepository = memberRepository;
            _emberShipRepository = emberShipRepository;
            _planRepository = planRepository;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _memberRepository.GetAll();
            if (members == null || !members.Any())
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
            if (member == null)
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

            var activeMemberShip = _emberShipRepository.GetAll(ms => ms.MemberId == id && ms.Status == "Active").FirstOrDefault();
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
                var emailExists = _memberRepository.GetAll(m => m.Email == createMember.Email).Any();

                var phoneExists = _memberRepository.GetAll(m => m.Phone == createMember.Phone).Any();

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
    }
}
