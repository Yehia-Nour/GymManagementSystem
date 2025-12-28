using GymManagement.BLL.ViewModels.AnalyticsViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync();
    }
}
