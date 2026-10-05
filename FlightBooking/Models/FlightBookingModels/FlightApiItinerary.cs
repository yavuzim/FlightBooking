using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiItinerary
    {
        [JsonPropertyName("departure_time")]
        public string? DepartureTime { get; set; }

        [JsonPropertyName("arrival_time")]
        public string? ArrivalTime { get; set; }

        [JsonPropertyName("duration")]
        public FlightApiDuration? Duration { get; set; }

        [JsonPropertyName("flights")]
        public List<FlightApiLeg>? Flights { get; set; }

        [JsonPropertyName("layovers")]
        public List<FlightApiLayover>? Layovers { get; set; }

        [JsonPropertyName("stops")]
        public int Stops { get; set; }

        [JsonPropertyName("airline_logo")]
        public string? AirlineLogo { get; set; }

        // price bazen sayı (1410), bazen "unavailable" string geliyor.
        // Bu yüzden JsonElement olarak alıp sonra normalize ediyoruz.
        [JsonPropertyName("price")]
        public System.Text.Json.JsonElement Price { get; set; }

    }
}
