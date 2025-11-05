using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberSessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
