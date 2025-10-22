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

            bool result = _memberService.CreateMember(createMember);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Create, Check Phone and Email";

            return RedirectToAction(nameof(Index));
        }

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id, MemberToUpdaterViewModel editMember)
        {
            if (!ModelState.IsValid)
                return View(editMember);

            var result = _memberService.UpdateMemberDetials(id, editMember);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Update, Check Phone and Email";

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;

            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var result = _memberService.DeleteMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Delete";

            return RedirectToAction(nameof(Index));
        }
    }
}
