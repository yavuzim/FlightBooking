namespace FlightBooking.Dtos.PassengerDtos
{
    public class CreatePassengerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PassengerType { get; set; }
    }
}
