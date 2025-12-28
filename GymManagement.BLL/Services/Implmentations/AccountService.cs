using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AccountViewModels;
using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.BLL.Services.Implmentations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> ValidateUserAsync(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
                return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            return isPasswordValid ? user : null;
        }
    }
}
