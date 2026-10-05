using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiAirport
    {
        [JsonPropertyName("airport_code")]
        public string? AirportCode { get; set; }

        [JsonPropertyName("airport_name")]
        public string? AirportName { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }
    }
}
