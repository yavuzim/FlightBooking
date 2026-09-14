using FlightBooking.Dtos.BookingDtos;
using FlightBooking.Entities;

namespace FlightBooking.Services.BookingServices
{
    public interface IBookingService
    {
        Task CreateBookingAsync(CreateBookingDto dto);
        Task<Booking> GetBookingByPassengerIdAsync(string passengerId);
    }
}
