using System.Text.Json;
using FlightBooking.Models;
using FlightBooking.Services.AirportServices;

namespace FlightBooking.Services
{
    public class AirportService : IAirportService
    {
        private readonly HttpClient _client;

        // HttpClient dependency injection ile geliyor (aşağıda Program.cs'de kaydedeceğiz)
        public AirportService(HttpClient client)
        {
            _client = client;
        }

        // Bir şehir sorgusu için tüm havalimanlarını döndürür
        public async Task<List<AirportResult>> SearchAirportsAsync(string query)
        {
            var results = new List<AirportResult>();

            if (string.IsNullOrWhiteSpace(query))
                return results;

            // API isteğini kur
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://google-flights2.p.rapidapi.com/api/v1/searchAirport" +
                    $"?query={Uri.EscapeDataString(query)}&language_code=tr-TR&country_code=TR"),
                Headers =
                {
                    { "x-rapidapi-key", "630ce9cc86msh271c60cffe62d5ep1b514djsn0fe292593744" },
                    { "x-rapidapi-host", "google-flights2.p.rapidapi.com" },
                },
            };

            using var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            // JSON'u modele çevir
            var parsed = JsonSerializer.Deserialize<AirportSearchResponse>(body);

            if (parsed?.Data == null)
                return results;

            // data[] -> her şehir -> list[] -> her havalimanı dolaş
            foreach (var data in parsed.Data)
            {
                foreach (var airport in data.List)
                {
                    // Sadece gerçek havalimanlarını al ve IATA kodu 3 harfli olanları
                    if (airport.Type == "airport" && !string.IsNullOrWhiteSpace(airport.Id))
                    {
                        results.Add(new AirportResult
                        {
                            Iata = airport.Id,                 // <-- IATA kodu burada
                            AirportName = airport.Title ?? "",
                            City = airport.City ?? data.City ?? "",
                            Title = data.Title ?? ""
                        });
                    }
                }
            }

            return results;
        }

        // Sadece ilk (en yakın) havalimanının IATA'sını isteyen basit senaryo için
        public async Task<AirportResult?> GetFirstIataAsync(string query)
        {
            var list = await SearchAirportsAsync(query);
            return list.FirstOrDefault();
        }
    }
}

