using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiData
    {
        [JsonPropertyName("itineraries")]
        public FlightApiItineraries? Itineraries { get; set; }

    }
}
