using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AccountViewModels;
using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _accountService.ValidateUser(model);
            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);
            }

            var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;
            if (result.IsNotAllowed) 
                ModelState.AddModelError("InvalidLogin", "Your Account is Not Allowed");
            if (result.IsLockedOut) 
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked Out");
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login");
        }
    }
}
