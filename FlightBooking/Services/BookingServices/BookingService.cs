using FlightBooking.Dtos.BookingDtos;
using FlightBooking.Entities;
using FlightBooking.Services.BookingServices;
using FlightBooking.Settings;
using MongoDB.Driver;

public class BookingService : IBookingService
{
    private readonly IMongoCollection<Booking> _bookingCollection;
    private readonly IMongoCollection<Flight> _flightCollection;

    public BookingService(IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);

        _bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);
        _flightCollection = database.GetCollection<Flight>(_databaseSettings.FlightCollectionName);
    }

    public async Task CreateBookingAsync(CreateBookingDto dto)
    {
        var flight = await _flightCollection
            .Find(x => x.FlightId == dto.FlightId)
            .FirstOrDefaultAsync();

        var passengerCount = dto.Passengers.Count;

        var passengers = dto.Passengers.Select(x => new Passenger
        {
            Name = x.Name,
            Surname = x.Surname,
            BirthDate = x.BirthDate,
            Gender = x.Gender,
            PassengerType = x.PassengerType
        }).ToList();

        var totalPrice = passengerCount * flight.BasePrice;
        var pnr = await GenerateUniquePnrAsync();

        var booking = new Booking
        {
            FlightId = dto.FlightId,
            Passengers = passengers,

            ContactName = dto.ContactName,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,

            TotalPrice = totalPrice,
            BookingDate = DateTime.Now,
            Status = "Confirmed",
            PnrNumber= pnr,
        };
        await _bookingCollection.InsertOneAsync(booking);

        //// 🔥 7. Koltuk düş
        //var update = Builders<Flight>.Update
        //    .Inc(x => x.AvailableSeats, -passengerCount);

        //await _flightCollection.UpdateOneAsync(
        //    x => x.FlightId == dto.FlightId,
        //    update
        //);
    }

    public async Task<string> GetGateByPassengerIdAsync(string passengerId)
    {
        var booking = await _bookingCollection
            .Find(x => x.Passengers.Any(p => p.PassengerId == passengerId))
            .FirstOrDefaultAsync();

        if (booking == null)
            return null;

        var passenger = booking.Passengers.FirstOrDefault(p=>p.PassengerId==passengerId);

        return passenger.Gate;
    }

    public async Task<(string Name, string Surname)> GetPassengerByIdAsync(string passengerId)
    {
       
        var booking = await _bookingCollection
            .Find(x => x.Passengers.Any(p => p.PassengerId == passengerId))
            .FirstOrDefaultAsync();

        if (booking == null)
            return (null, null);

        var passenger = booking.Passengers.FirstOrDefault(p => p.PassengerId == passengerId);
        return passenger == null ? (null, null) : (passenger.Name, passenger.Surname);
    }

    public async Task<string> GetPnrByPassengerIdAsync(string passengerId)
    {
        var booking = await _bookingCollection
             .Find(x => x.Passengers.Any(p => p.PassengerId == passengerId))
             .FirstOrDefaultAsync();

        if (booking == null)
            return null;

        return booking.PnrNumber;
    }

    private async Task<string> GenerateUniquePnrAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        string pnr;
        bool exists;

        do
        {
            pnr = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            exists = await _bookingCollection
                .Find(x => x.PnrNumber == pnr)
                .AnyAsync();

        } while (exists);

        return pnr;
    }
}