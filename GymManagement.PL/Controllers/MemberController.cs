using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<ActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        public async Task<ActionResult> MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = await _memberService.GetMemberDetialsAsync(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public async Task<ActionResult> HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of HealthRecord Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var healthRecord = await _memberService.GetMemberHealthRecordDetialsAsync(id);
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
        public async Task<ActionResult> CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(nameof(Create), createMember);
            }

            bool result = await _memberService.CreateMemberAsync(createMember);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Create, Check Data and Missing Fields";

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = await _memberService.GetMemberToUpdateAsync(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }
        [HttpPost]
        public async Task<ActionResult> MemberEdit([FromRoute] int id, MemberToUpdateViewModel editMember)
        {
            if (!ModelState.IsValid)
                return View(editMember);

            var result = await _memberService.UpdateMemberDetialsAsync(id, editMember);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Update, Check Phone and Email";

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = await _memberService.GetMemberDetialsAsync(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfuly";
            else
                TempData["ErrorMessage"] = "Member Failed to Delete";

            return RedirectToAction(nameof(Index));
        }
    }
}
