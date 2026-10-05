using System.Text.Json.Serialization;

namespace FlightBooking.Models.FlightBookingModels
{
    public class FlightApiCarbon
    {
        [JsonPropertyName("CO2e")]
        public int Co2e { get; set; }

        [JsonPropertyName("difference_percent")]
        public int DifferencePercent { get; set; }

    }
}
