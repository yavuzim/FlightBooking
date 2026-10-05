using System.Text.Json.Serialization;

namespace FlightBooking.Models
{
    public class AirportItem
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }          

        [JsonPropertyName("type")]
        public string? Type { get; set; }        

        [JsonPropertyName("title")]
        public string? Title { get; set; }       

        [JsonPropertyName("subtitle")]
        public string? Subtitle { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("distance")]
        public string? Distance { get; set; }
    }

}
