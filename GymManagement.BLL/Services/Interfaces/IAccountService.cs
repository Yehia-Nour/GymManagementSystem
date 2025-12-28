using GymManagement.BLL.ViewModels.AccountViewModels;
using GymManagement.DAL.Entities;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser?> ValidateUserAsync(LoginViewModel loginViewModel);
    }
}
