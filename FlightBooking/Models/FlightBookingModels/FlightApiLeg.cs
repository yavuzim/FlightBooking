using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiLeg
    {
        [JsonPropertyName("departure_airport")]
        public FlightApiAirport? DepartureAirport { get; set; }

        [JsonPropertyName("arrival_airport")]
        public FlightApiAirport? ArrivalAirport { get; set; }

        [JsonPropertyName("airline")]
        public string? Airline { get; set; }

        [JsonPropertyName("airline_logo")]
        public string? AirlineLogo { get; set; }

        [JsonPropertyName("flight_number")]
        public string? FlightNumber { get; set; }

        [JsonPropertyName("aircraft")]
        public string? Aircraft { get; set; }

        [JsonPropertyName("legroom")]
        public string? Legroom { get; set; }

        [JsonPropertyName("duration")]
        public FlightApiDuration? Duration { get; set; }

    }
}
