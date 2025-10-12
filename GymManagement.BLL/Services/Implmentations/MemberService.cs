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

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
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
