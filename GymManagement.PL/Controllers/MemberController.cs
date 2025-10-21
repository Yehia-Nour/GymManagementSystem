using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var member = _memberService.GetMemberDetials(id);
            if (member is null)
                return RedirectToAction(nameof(Index));

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var healthRecord = _memberService.GetMemberHealthRecordDetials(id);
            if (healthRecord is null)
                return RedirectToAction(nameof(Index));

            return View(healthRecord);
        }
    }
}
