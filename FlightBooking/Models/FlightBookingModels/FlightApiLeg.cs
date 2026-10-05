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

    }
}
