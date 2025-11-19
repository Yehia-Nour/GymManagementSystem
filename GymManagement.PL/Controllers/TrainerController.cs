using GymManagement.BLL.Services.Implmentations;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<ActionResult> Index()
        {
            var trainers = await _trainerService.GetAllTrainersAsync();
            return View(trainers);
        }

        public async Task<ActionResult> TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(nameof(Create), createTrainer);
            }

            bool result = await _trainerService.CreateTrainerAsync(createTrainer);
            if (result)
                TempData["SuccessMessage"] = "Trainer Created Successfuly";
            else
                TempData["ErrorMessage"] = "Trainer Failed to Create, Check Data and Missing Fields";

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerToUpdateAsync(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        [HttpPost]
        public async Task<ActionResult> TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel editTrainer)
        {
            if (!ModelState.IsValid)
                return View(editTrainer);

            var result = await _trainerService.UpdateTrainerAsync(id, editTrainer);
            if (result)
                TempData["SuccessMessage"] = "Trainer Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Trainer Failed to Update, Check Phone and Email";

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var result = await _trainerService.DeleteTrainerAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfuly";
            else
                TempData["ErrorMessage"] = "Trainer Failed to Delete";

            return RedirectToAction(nameof(Index));
        }
    }
}
