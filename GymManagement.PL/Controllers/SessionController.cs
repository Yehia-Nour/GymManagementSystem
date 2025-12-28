using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<ActionResult> Index()
        {
            var sessions = await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = await _sessionService.GetSessionDetailsAsync(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public async Task<ActionResult> Create()
        {
            await LoadTrainersDropDownsAsync();
            await LoadCategoriesDropDownsAsync();

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                await LoadTrainersDropDownsAsync();
                await LoadCategoriesDropDownsAsync();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(createSession);
            }

            bool result = await _sessionService.CreateSessionAsync(createSession);
            if (result)
                TempData["SuccessMessage"] = "Session Created Successfuly";
            else
            {
                await LoadTrainersDropDownsAsync();
                await LoadCategoriesDropDownsAsync();
                return View(createSession);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = await _sessionService.GetSessionToUpdateAsync(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            await LoadTrainersDropDownsAsync();

            return View(session);
        }
        [HttpPost]
        public async Task<ActionResult> Edit([FromRoute] int id, UpdateSessionViewModel editSession)
        {
            if (!ModelState.IsValid)
            {
                await LoadTrainersDropDownsAsync();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(editSession);
            }

            var result = await _sessionService.UpdateSessionAsync(editSession, id);
            if (result)
                TempData["SuccessMessage"] = "Session Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Session Failed to Update";

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = await _sessionService.GetSessionDetailsAsync(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = id;
            return View(session);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await _sessionService.DeleteSessionAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfuly";
            else
                TempData["ErrorMessage"] = "Session Failed to Delete";

            return RedirectToAction(nameof(Index));
        }

        async Task LoadTrainersDropDownsAsync()
        {
            var trainers = await _sessionService.GetAllTrainersForDropDownAsync();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        async Task LoadCategoriesDropDownsAsync()
        {
            var categories = await _sessionService.GetAllCategoriesForDropDownAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}
