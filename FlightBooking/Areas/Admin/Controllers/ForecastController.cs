using FlightBooking.Services.MachineLearningServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ForecastController : Controller
    {
        private readonly MongoFlightDataService _mongoFlightDataService;
        private readonly FlightMlService _flightMlService;

        public ForecastController(MongoFlightDataService mongoFlightDataService, FlightMlService flightMlService)
        {
            _mongoFlightDataService = mongoFlightDataService;
            _flightMlService = flightMlService;
        }

        public async Task<IActionResult> TrainModel()
        {
            var mlData = await _mongoFlightDataService.ConvertToMlDataAsync();
            _flightMlService.Train(mlData);
            ViewBag.Message = "Model başarıyla eğitildi.";
            return View();
        }
    }
}
