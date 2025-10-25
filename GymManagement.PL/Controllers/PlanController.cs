using GymManagement.BLL.Services.Implmentations;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plan Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanDetails(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plan Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Wrong Data", "Check Data Validation");
                return View(updatePlan);
            }

            var result = _planService.UpdatePlan(id, updatePlan);
            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Plan Failed to Update";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Activate(int id )
        {
            var result = _planService.ToggleStatus(id);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to change Plan Status";

            return RedirectToAction(nameof(Index));
        }
    }
}
