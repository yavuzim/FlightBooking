namespace FlightBooking.Dtos.FlightSearchDtos
{
    public class FlightCardDto
    {
        public string Airline { get; set; } = "";
        public string AirlineLogo { get; set; } = "";
        public string DepartureTime { get; set; } = "";   
        public string ArrivalTime { get; set; } = "";     
        public string DepartureAirport { get; set; } = ""; 
        public string ArrivalAirport { get; set; } = "";  
        public string DurationText { get; set; } = "";    
        public int Stops { get; set; }                    
        public List<string> LayoverCities { get; set; } = new();
        public string Price { get; set; } = "";           

    }
}
