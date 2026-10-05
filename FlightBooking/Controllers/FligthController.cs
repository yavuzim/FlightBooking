using FlightBooking.Dtos.FlightSearchDtos;
using FlightBooking.Services.AirportServices;
using FlightBooking.Services.FlightSearchServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    public class FligthController : Controller
    {
        private readonly IAirportService _airportService;
        private readonly IFlightSearchService _flightSearchService;

        public FligthController(IAirportService airportService, IFlightSearchService flightSearchService)
        {
            _airportService = airportService;
            _flightSearchService = flightSearchService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchFlights(string fromCity, string toCity, string depart, int adults = 1, string cabin = "economy", string currency = "TRY")
        {
            var result = new FlightSearchResultDto { Currency = currency };

            try
            {
                // 1) Şehir isimlerinden IATA çöz
                var from = await _airportService.GetFirstIataAsync(fromCity);
                var to = await _airportService.GetFirstIataAsync(toCity);

                if (from?.Iata == null || to?.Iata == null)
                {
                    result.Success = false;
                    result.Error = "Kalkış veya varış için havalimanı bulunamadı.";
                    return Json(result);
                }

                result.FromIata = from.Iata;
                result.FromAirport = from.AirportName;
                result.ToIata = to.Iata;
                result.ToAirport = to.AirportName;

                // 2) IATA'larla uçuşları getir
                result.Flights = await _flightSearchService.SearchAsync(from.Iata, to.Iata, depart, adults, cabin, currency);

                result.Success = true;
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
                return Json(result);
            }
        }



        [HttpGet]
        public async Task<IActionResult> SearchAirport(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            try
            {
                var airports = await _airportService.SearchAirportsAsync(query);
                return Json(airports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResolveIata(string fromCity, string toCity)
        {
            var from = await _airportService.GetFirstIataAsync(fromCity);
            var to = await _airportService.GetFirstIataAsync(toCity);

            return Json(new
            {
                fromIata = from?.Iata,
                fromAirport = from?.AirportName,
                toIata = to?.Iata,
                toAirport = to?.AirportName
            });
        }

    }
}
