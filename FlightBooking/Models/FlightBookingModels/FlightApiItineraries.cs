using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiItineraries
    {
        [JsonPropertyName("topFlights")]
        public List<FlightApiItinerary>? TopFlights { get; set; }

        [JsonPropertyName("otherFlights")]
        public List<FlightApiItinerary>? OtherFlights { get; set; }

    }
}
