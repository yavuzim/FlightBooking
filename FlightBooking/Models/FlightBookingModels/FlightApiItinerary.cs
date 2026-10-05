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

        [JsonPropertyName("price")]
        public System.Text.Json.JsonElement Price { get; set; }

        // ---- YENİ: bagaj + karbon ----
        [JsonPropertyName("bags")]
        public FlightApiBags? Bags { get; set; }

        [JsonPropertyName("carbon_emissions")]
        public FlightApiCarbon? CarbonEmissions { get; set; }
    }
}
