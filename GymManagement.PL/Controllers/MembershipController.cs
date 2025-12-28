using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<ActionResult> Index()
        {
            var memberships = await _membershipService.GetAllMembershipsAsync();
            return View(memberships);
        }

        public async Task<ActionResult> Create()
        {
            await LoadMembersDropDowns();
            await LoadPlansDropDowns();

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateMembershipViewModel createMembership)
        {
            if (!ModelState.IsValid)
            {
                await LoadMembersDropDowns();
                await LoadPlansDropDowns();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(createMembership);
            }

            bool result = await _membershipService.CraeteMembershipAsync(createMembership);
            if (result)
                TempData["SuccessMessage"] = "Membership Created Successfuly";
            else
                TempData["ErrorMessage"] = "Membership Failed to Create, Check Data";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int id)
        {
            if (id <= 0)
                TempData["ErrorMessage"] = "Id of Membership Can't be 0 or Nigative Number";

            var isDeleted = await _membershipService.DeleteMembershipAsync(id);

            if (!isDeleted)
                TempData["ErrorMessage"] = "Membership Can't be Delete";


            return RedirectToAction(nameof(Index));
        }

        async Task LoadMembersDropDowns()
        {
            var members = await _membershipService.GetAllMembersForDropDownAsync();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }
        async Task LoadPlansDropDowns()
        {
            var plans = await _membershipService.GetAllPlansForDropDownAsync();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
    }
}
