using AutoMapper;
using GymManagement.BLL.ViewModels.MemberSessionViewModels;
using GymManagement.DAL.Entities;

namespace GymManagement.BLL.MappingProfiles
{
    internal class MemberSessionProfile : Profile
    {
        public MemberSessionProfile()
        {
            CreateMap<MemberSession, MemberForSessionViewModel>()
                    .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                    .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToString()));

            CreateMap<CreateBookingViewModel, MemberSession>();
        }
    }
}
