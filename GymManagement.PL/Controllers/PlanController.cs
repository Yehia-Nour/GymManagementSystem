using GymManagement.BLL.Services.Implmentations;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<ActionResult> Index()
        {
            var plans = await _planService.GetAllPlansAsync();
            return View(plans);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plan Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = await _planService.GetPlanDetailsAsync(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plan Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = await _planService.GetPlanToUpdateAsync(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        [HttpPost]
        public async Task<ActionResult> Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Wrong Data", "Check Data Validation");
                return View(updatePlan);
            }

            var result = await _planService.UpdatePlanAsync(id, updatePlan);
            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Plan Failed to Update";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Activate(int id )
        {
            var result = await _planService.ToggleStatusAsync(id);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to change Plan Status";

            return RedirectToAction(nameof(Index));
        }
    }
}
