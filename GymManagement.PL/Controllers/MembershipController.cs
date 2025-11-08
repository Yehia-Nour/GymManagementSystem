using GymManagement.BLL.Services.Implmentations;
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

        public ActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships);
        }

        public ActionResult Create()
        {
            LoadMembersDropDowns();
            LoadPlansDropDowns();

            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMembershipViewModel createMembership)
        {
            if (!ModelState.IsValid)
            {
                LoadMembersDropDowns();
                LoadPlansDropDowns();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(createMembership);
            }

            bool result = _membershipService.CraeteMembership(createMembership);
            if (result)
                TempData["SuccessMessage"] = "Membership Created Successfuly";
            else
                TempData["ErrorMessage"] = "Membership Failed to Create, Check Data";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            if (id <= 0)
                TempData["ErrorMessage"] = "Id of Membership Can't be 0 or Nigative Number";

            var isDeleted = _membershipService.DeleteMembership(id);

            if (!isDeleted)
                TempData["ErrorMessage"] = "Membership Can't be Delete";


            return RedirectToAction(nameof(Index));
        }

        void LoadMembersDropDowns()
        {
            var members = _membershipService.GetAllMembersForDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }
        void LoadPlansDropDowns()
        {
            var plans = _membershipService.GetAllPlansForDropDown();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
    }
}
