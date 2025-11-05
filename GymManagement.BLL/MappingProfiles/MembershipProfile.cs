using AutoMapper;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.MappingProfiles
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<MemberShip, MembershipViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Member, MemberSelectViewModel>();

            CreateMap<Plan, PlanSelectViewModel>();

            CreateMap<CreateMembershipViewModel, MemberShip>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
