using FlightBooking.Dtos.FlightSearchDtos;

namespace FlightBooking.Services.FlightSearchServices
{
    public interface IFlightSearchService
    {
        Task<List<FlightCardDto>> SearchAsync(string fromIata, string toIata, string outboundDate, int adults, string cabin, string currency);
    }
}
