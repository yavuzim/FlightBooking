using FlightBooking.Dtos.BookingDtos;
using FlightBooking.Entities;

namespace FlightBooking.Services.BookingServices
{
    public interface IBookingService
    {
        Task CreateBookingAsync(CreateBookingDto dto);
        Task<(string Name, string Surname)> GetPassengerByIdAsync(string passengerId);
        Task<string> GetPnrByPassengerIdAsync(string passengerId);
        Task<string> GetGateByPassengerIdAsync(string passengerId);
    }
}
