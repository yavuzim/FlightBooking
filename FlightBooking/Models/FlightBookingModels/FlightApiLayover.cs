using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiLayover
    {
        [JsonPropertyName("airport_code")]
        public string? AirportCode { get; set; }

        [JsonPropertyName("airport_name")]
        public string? AirportName { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("duration_label")]
        public string? DurationLabel { get; set; }
    }
}
