using GymManagement.BLL.Services.Implmentations;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymManagement.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public ActionResult Create()
        {
            LoadTrainersDropDowns();
            LoadCategoriesDropDowns();

            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(createSession);
            }

            bool result = _sessionService.CreateSession(createSession);
            if (result)
                TempData["SuccessMessage"] = "Session Created Successfuly";
            else
                return View(createSession);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainersDropDowns();

            return View(session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel editSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();

                ModelState.AddModelError("DataInvaild", "Check Data and Missing Fields");
                return View(editSession);
            }

            var result = _sessionService.UpdateSession(editSession, id);
            if (result)
                TempData["SuccessMessage"] = "Session Updated Successfuly";
            else
                TempData["ErrorMessage"] = "Session Failed to Update";

            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Session Can't be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = id;
            return View(session);
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.DeleteSession(id);

            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfuly";
            else
                TempData["ErrorMessage"] = "Session Failed to Delete";

            return RedirectToAction(nameof(Index));
        }

        void LoadTrainersDropDowns()
        {
            var trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        void LoadCategoriesDropDowns()
        {
            var categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}
