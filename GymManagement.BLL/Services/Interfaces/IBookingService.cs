using GymManagement.BLL.ViewModels.MemberSessionViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsWithTrainerAndCategoryAsync();
        Task<IEnumerable<MemberForSessionViewModel>> GetAllMembersForUpcomingSessionAsync(int id);
        Task<IEnumerable<MemberForSessionViewModel>> GetAllMembersForOngoingSessionAsync(int id);
        Task<bool> CreateBookingAsync(CreateBookingViewModel createBookingViewModel);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropdownAsync(int id);
        Task<bool> MemberAttendedAsync(MemberAttendOrCancelViewModel model);
        Task<bool> CancelBookingAsync(MemberAttendOrCancelViewModel model);
    }
}
