namespace FlightBooking.Dtos.FlightSearchDtos
{
    public class FlightSegmentDto
    {
        public string Airline { get; set; } = "";
        public string AirlineLogo { get; set; } = "";
        public string FlightNumber { get; set; } = "";
        public string Aircraft { get; set; } = "";
        public string Legroom { get; set; } = "";
        public string DurationText { get; set; } = "";

        public string DepartureTime { get; set; } = "";
        public string DepartureCode { get; set; } = "";
        public string DepartureName { get; set; } = "";

        public string ArrivalTime { get; set; } = "";
        public string ArrivalCode { get; set; } = "";
        public string ArrivalName { get; set; } = "";
    }
}
