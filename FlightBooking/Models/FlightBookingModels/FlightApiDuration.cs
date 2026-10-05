using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiDuration
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
