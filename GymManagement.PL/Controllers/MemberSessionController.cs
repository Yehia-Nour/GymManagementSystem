using GymManagement.BLL.Services.Implmentations;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IBookingService _bookingService;

        public MemberSessionController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<ActionResult> Index()
        {
            var sessions = await _bookingService.GetAllSessionsWithTrainerAndCategoryAsync();  
            return View(sessions);
        }

        public async Task<ActionResult> GetMembersForUpcomingSession(int id)
        {
            var members = await _bookingService.GetAllMembersForUpcomingSessionAsync(id);
            return View(members);
        }
        public async Task<ActionResult> GetMembersForOngoingSession(int id)
        {
            var members = await _bookingService.GetAllMembersForOngoingSessionAsync(id);
            return View(members);
        }

        public async Task<ActionResult> Create(int id)
        {
            var members = await _bookingService.GetMembersForDropdownAsync(id);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBookingViewModel model)
        {
            var result = await _bookingService.CreateBookingAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking Created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Booking.";
            }

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<ActionResult> Attended(MemberAttendOrCancelViewModel model)
        {
            var result = await _bookingService.MemberAttendedAsync(model);

            if (result)
                TempData["SuccessMessage"] = "Member attended successfully";
            else
                TempData["ErrorMessage"] = "Member attendance can't be marked";

            return RedirectToAction(nameof(GetMembersForOngoingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(MemberAttendOrCancelViewModel model)
        {
            var result = await _bookingService.CancelBookingAsync(model);

            if (result)
                TempData["SuccessMessage"] = "Booking cancelled successfully";
            else
                TempData["ErrorMessage"] = "Booking can't be cancelled";
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }
    }
}
