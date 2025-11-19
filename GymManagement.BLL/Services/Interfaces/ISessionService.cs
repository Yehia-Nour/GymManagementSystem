using GymManagement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
