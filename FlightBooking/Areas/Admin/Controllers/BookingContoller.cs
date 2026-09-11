using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingContoller : Controller
    {
        public IActionResult CreateBooking()
        {
            return View();
        }
        public IActionResult BookingList()
        {
            return View();
        }
    }
}
