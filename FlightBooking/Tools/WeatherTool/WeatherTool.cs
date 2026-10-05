using FlightBooking.Dtos.AgentDtos;
using FlightBooking.Dtos.WeatherDtos;
using System.Text.Json;

namespace FlightBooking.Tools.WeatherTool
{
    public class WeatherTool : IWeatherTool
    {
        private readonly HttpClient _httpClient;

        private const string RapidApiKey = "84262ba1d8mshde14ba692169d32p14788djsnbba8d7033649";
        private const string RapidApiHost = "yahoo-weather5.p.rapidapi.com";
        private const string BaseUrl = "https://yahoo-weather5.p.rapidapi.com/weather";

        public WeatherTool(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResult> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException(
                    "Şehir bilgisi boş bırakılamaz.",
                    nameof(city));
            }

            var encodedCity = Uri.EscapeDataString(city);

            var requestUrl =
                $"{BaseUrl}?location={encodedCity}&format=json&u=c";

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl)
            };

            request.Headers.Add("x-rapidapi-key", RapidApiKey);
            request.Headers.Add("x-rapidapi-host", RapidApiHost);

            using var response =
                await _httpClient.SendAsync(request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Hava durumu API isteği başarısız oldu. " +
                    $"Durum kodu: {(int)response.StatusCode}. " +
                    $"Cevap: {responseContent}");
            }

            using var document = JsonDocument.Parse(responseContent);

            var root = document.RootElement;

            var location =
                root.GetProperty("location");

            var currentObservation =
                root.GetProperty("current_observation");

            var wind =
                currentObservation.GetProperty("wind");

            var atmosphere =
                currentObservation.GetProperty("atmosphere");

            var astronomy =
                currentObservation.GetProperty("astronomy");

            var condition =
                currentObservation.GetProperty("condition");

            var weatherResult = new WeatherResult
            {
                City = location
                    .GetProperty("city")
                    .GetString() ?? city,

                Country = location
                    .GetProperty("country")
                    .GetString() ?? string.Empty,

                TimeZoneId = location
                    .GetProperty("timezone_id")
                    .GetString() ?? string.Empty,

                Temperature = condition
                    .GetProperty("temperature")
                    .GetDecimal(),

                Condition = condition
                    .GetProperty("text")
                    .GetString() ?? "Bilinmiyor",

                Humidity = atmosphere
                    .GetProperty("humidity")
                    .GetInt32(),

                WindSpeed = wind
                    .GetProperty("speed")
                    .GetDouble(),

                WindDirection = wind
                    .GetProperty("direction")
                    .GetString() ?? string.Empty,

                Visibility = atmosphere
                    .GetProperty("visibility")
                    .GetInt32(),

                Pressure = atmosphere
                    .GetProperty("pressure")
                    .GetInt32(),

                Sunrise = astronomy
                    .GetProperty("sunrise")
                    .GetString() ?? string.Empty,

                Sunset = astronomy
                    .GetProperty("sunset")
                    .GetString() ?? string.Empty
            };

            if (root.TryGetProperty(
                    "forecasts",
                    out var forecastsElement))
            {
                foreach (var forecast in
                         forecastsElement.EnumerateArray())
                {
                    weatherResult.Forecasts.Add(
                        new WeatherForecastResult
                        {
                            Day = forecast
                                .GetProperty("day")
                                .GetString() ?? string.Empty,

                            Date = forecast
                                .GetProperty("date")
                                .GetInt64(),

                            Low = forecast
                                .GetProperty("low")
                                .GetInt32(),

                            High = forecast
                                .GetProperty("high")
                                .GetInt32(),

                            Condition = forecast
                                .GetProperty("text")
                                .GetString() ?? string.Empty
                        });
                }
            }

            return weatherResult;
        }

    }
}
