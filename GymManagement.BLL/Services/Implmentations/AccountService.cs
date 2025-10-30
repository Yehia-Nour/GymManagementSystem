using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AccountViewModels;
using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user == null)
                return null;

            var isPasswordValid = _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;

            return isPasswordValid ? user : null;
        }
    }
}
