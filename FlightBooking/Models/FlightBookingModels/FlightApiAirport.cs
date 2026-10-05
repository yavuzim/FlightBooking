using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiAirport
    {
        [JsonPropertyName("airport_code")]
        public string? AirportCode { get; set; }

    }
}
