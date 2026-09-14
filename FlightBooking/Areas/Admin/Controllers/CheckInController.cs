using FlightBooking.Services.BookingServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CheckInController : Controller
    {
        private readonly IBookingService _bookingService;

        public CheckInController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(string passengerId)
        {
            ViewBag.FlightNumber = TempData["FlightNumber"];
            ViewBag.DepartureTime = TempData["DepartureTime"];
            ViewBag.ArrivalTime = TempData["ArrivalTime"];

            var booking = await _bookingService.GetBookingByPassengerIdAsync(passengerId);

            var passenger = booking?.Passengers
                .FirstOrDefault(x => x.PassengerId == passengerId);

            ViewBag.PassengerName = passenger.Name;
            ViewBag.Surname = passenger.Surname;

            return View();
        }
    }
}