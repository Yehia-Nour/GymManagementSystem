using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Member = GymManagement.DAL.Entities.Member;

namespace GymManagement.BLL.MappingProfiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<Member, MemberWithDetailsViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.MemberShipStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.MemberShipEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.PlanName, opt => opt.Ignore());

            CreateMap<CreateMemberViewModel, Member>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.HealthRecord.Height, opt => opt.MapFrom(src => src.HealthRecordViewModel.Height))
                .ForPath(dest => dest.HealthRecord.Weight, opt => opt.MapFrom(src => src.HealthRecordViewModel.Weight))
                .ForPath(dest => dest.HealthRecord.BloodType, opt => opt.MapFrom(src => src.HealthRecordViewModel.BloodType))
                .ForPath(dest => dest.HealthRecord.Note, opt => opt.MapFrom(src => src.HealthRecordViewModel.Note ?? ""));

            CreateMap<Member, MemberToUpdaterViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<MemberToUpdaterViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City));

            CreateMap<HealthRecord, HealthRecordViewModel>();
        }
    }
}
