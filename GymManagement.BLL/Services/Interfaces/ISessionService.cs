using GymManagement.BLL.ViewModels.SessionViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync();
        Task<SessionViewModel?> GetSessionDetailsAsync(int id);
        Task<bool> CreateSessionAsync(CreateSessionViewModel createSession);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int id);
        Task<bool> UpdateSessionAsync(UpdateSessionViewModel updateSession, int id);
        Task<bool> DeleteSessionAsync(int id);
        Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync();
        Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropDownAsync();
    }
}
