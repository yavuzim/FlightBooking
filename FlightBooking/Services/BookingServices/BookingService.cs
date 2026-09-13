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
        // 🔥 1. Flight çek
        var flight = await _flightCollection
            .Find(x => x.FlightId == dto.FlightId)
            .FirstOrDefaultAsync();

        //if (flight == null)
        //    throw new Exception("Uçuş bulunamadı");

        // 🔥 2. Yolcu sayısı
        var passengerCount = dto.Passengers.Count;

        //// 🔥 3. Koltuk kontrol
        //if (flight.AvailableSeats < passengerCount)
        //    throw new Exception("Yeterli koltuk yok");

        // 🔥 4. Passenger mapping
        var passengers = dto.Passengers.Select(x => new Passenger
        {
            Name = x.Name,
            Surname = x.Surname,
            BirthDate = x.BirthDate,
            Gender = x.Gender,
            PassengerType = x.PassengerType
        }).ToList();

        // 🔥 5. Fiyat hesaplama
        var totalPrice = passengerCount * flight.BasePrice;

        // 🔥 6. Booking oluştur
        var booking = new Booking
        {
            FlightId = dto.FlightId,
            Passengers = passengers,

            ContactName = dto.ContactName,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,

            TotalPrice = totalPrice,
            BookingDate = DateTime.Now,
            Status = "Confirmed"
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
}