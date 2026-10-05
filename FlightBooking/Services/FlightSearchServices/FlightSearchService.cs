using FlightBooking.Dtos.FlightSearchDtos;
using FlightBooking.Models.FlightBookingModels;
using System.Text.Json;

namespace FlightBooking.Services.FlightSearchServices
{
    public class FlightSearchService : IFlightSearchService
    {
        private readonly HttpClient _client;

        public FlightSearchService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<FlightCardDto>> SearchAsync(string fromIata, string toIata, string outboundDate, int adults, string cabin, string currency)
        {
            var cards = new List<FlightCardDto>();

            if (string.IsNullOrWhiteSpace(fromIata) || string.IsNullOrWhiteSpace(toIata))
                return cards;

            // API travel_class büyük harf bekliyor (ECONOMY, BUSINESS...)
            var travelClass = MapCabin(cabin);

            var url =
                $"https://google-flights2.p.rapidapi.com/api/v1/searchFlights" +
                $"?departure_id={Uri.EscapeDataString(fromIata)}" +
                $"&arrival_id={Uri.EscapeDataString(toIata)}" +
                $"&outbound_date={Uri.EscapeDataString(outboundDate)}" +
                $"&travel_class={travelClass}" +
                $"&adults={adults}" +
                $"&show_hidden=1" +
                $"&currency={Uri.EscapeDataString(currency)}" +
                $"&language_code=en-US&country_code=US&search_type=best";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", "630ce9cc86msh271c60cffe62d5ep1b514djsn0fe292593744" },
                    { "x-rapidapi-host", "google-flights2.p.rapidapi.com" },
                },
            };

            using var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var parsed = JsonSerializer.Deserialize<FlightApiResponse>(body);
            var itin = parsed?.Data?.Itineraries;
            if (itin == null) return cards;

            // topFlights + otherFlights hepsini birleştir
            var all = new List<FlightApiItinerary>();
            if (itin.TopFlights != null) all.AddRange(itin.TopFlights);
            if (itin.OtherFlights != null) all.AddRange(itin.OtherFlights);

            foreach (var it in all)
            {
                var firstLeg = it.Flights?.FirstOrDefault();
                var lastLeg = it.Flights?.LastOrDefault();

                cards.Add(new FlightCardDto
                {
                    Airline = firstLeg?.Airline ?? "",
                    AirlineLogo = it.AirlineLogo ?? firstLeg?.AirlineLogo ?? "",
                    DepartureTime = ExtractClock(it.DepartureTime),
                    ArrivalTime = ExtractClock(it.ArrivalTime),
                    DepartureAirport = firstLeg?.DepartureAirport?.AirportCode ?? fromIata,
                    ArrivalAirport = lastLeg?.ArrivalAirport?.AirportCode ?? toIata,
                    DurationText = it.Duration?.Text ?? "",
                    Stops = it.Stops,
                    LayoverCities = it.Layovers?
                        .Where(l => !string.IsNullOrWhiteSpace(l.City))
                        .Select(l => l.City!)
                        .ToList() ?? new List<string>(),
                    Price = NormalizePrice(it.Price)
                });
            }

            return cards;
        }

        private static string ExtractClock(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            var parts = raw.Split(' ', 2);
            return parts.Length == 2 ? parts[1] : raw;
        }
        private static string NormalizePrice(JsonElement price)
        {
            return price.ValueKind switch
            {
                JsonValueKind.Number => price.GetRawText(),
                JsonValueKind.String => price.GetString() ?? "unavailable",
                _ => "unavailable"
            };
        }
        private static string MapCabin(string cabin) => cabin?.ToLower() switch
        {
            "business" => "BUSINESS",
            "first" => "FIRST",
            "premium" => "PREMIUM_ECONOMY",
            _ => "ECONOMY"
        };
    }

}

