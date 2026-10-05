using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiLayover
    {
        [JsonPropertyName("city")]
        public string? City { get; set; }

    }
}
