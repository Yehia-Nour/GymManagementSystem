using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
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
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetials(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of HealthRecord Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var healthRecord = _memberService.GetMemberHealthRecordDetials(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(nameof(Create), createMember);
            }

            bool reault = _memberService.CreateMember(createMember);
            if (reault)
                TempData["SuccessMessage"] = "Member Created Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Create, Check Phone and Email";

            return RedirectToAction(nameof(Index));
        }
    }
}
