
using FlightBooking.AgentServices.CityDetectors;
using FlightBooking.AgentServices.IntentDetectors;
using FlightBooking.AgentServices.OpenAIServices;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.Dtos.AgentDtos;
using FlightBooking.Tools.WeatherTool;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IOpenAIService _openAIService;
        private readonly ITravelPromptBuilder _travelPromptBuilder;
        private readonly IIntentDetector _intentDetector;
        private readonly IWeatherTool _weatherTool;
        private readonly ICityExtractor _cityExtractor;

        public TravelAgentService(IOpenAIService openAIService, ITravelPromptBuilder travelPromptBuilder, IIntentDetector intentDetector, IWeatherTool weatherTool, ICityExtractor cityExtractor)
        {
            _openAIService = openAIService;
            _travelPromptBuilder = travelPromptBuilder;
            _intentDetector = intentDetector;
            _weatherTool = weatherTool;
            _cityExtractor = cityExtractor;
        }
        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            var intent = _intentDetector.Detect(prompt);

            string intentInstruction;

            switch (intent)
            {
                case TravelIntent.Weather:
                    {
                        var city = await _cityExtractor.ExtractCityAsync(prompt);

                        if (string.IsNullOrWhiteSpace(city))
                        {
                            intentInstruction =
                                "Kullanıcı hava durumu bilgisi istiyor ancak şehir belirtmemiş. " +
                                "Kullanıcıdan hangi şehrin hava durumunu öğrenmek istediğini sor.";

                            break;
                        }

                        var weatherResult = await _weatherTool.GetWeatherAsync(city);

                        var forecastText = string.Join(
                            "\n",
                            weatherResult.Forecasts.Select(x =>
                                $"{x.Day}: En düşük {x.Low}°C, " +
                                $"en yüksek {x.High}°C, durum: {x.Condition}"));

                        intentInstruction =
                            $"Kullanıcı hava durumu bilgisi istiyor.\n\n" +
                            $"Weather Tool tarafından sağlanan gerçek hava durumu verileri:\n" +
                            $"Şehir: {weatherResult.City}\n" +
                            $"Ülke: {weatherResult.Country}\n" +
                            $"Sıcaklık: {weatherResult.Temperature}°C\n" +
                            $"Durum: {weatherResult.Condition}\n" +
                            $"Nem: %{weatherResult.Humidity}\n" +
                            $"Rüzgar: {weatherResult.WindSpeed} km/sa, " +
                            $"{weatherResult.WindDirection}\n" +
                            $"Görüş mesafesi: {weatherResult.Visibility} km\n" +
                            $"Basınç: {weatherResult.Pressure} hPa\n" +
                            $"Gün doğumu: {weatherResult.Sunrise}\n" +
                            $"Gün batımı: {weatherResult.Sunset}\n\n" +
                            $"Gelecek gün tahminleri:\n{forecastText}\n\n" +
                            $"Yalnızca Weather Tool tarafından sağlanan verileri kullan. " +
                            $"Hava durumu veya sıcaklık uydurma. " +
                            $"Kullanıcının sorusuna göre kıyafet, şemsiye ve seyahat önerisi ver.";

                        break;
                    }

                case TravelIntent.Restaurant:
                    intentInstruction =
                        "Kullanıcı restoran önerisi istiyor.";
                    break;

                case TravelIntent.Hotel:
                    intentInstruction =
                        "Kullanıcı otel önerisi istiyor.";
                    break;

                case TravelIntent.Transportation:
                    intentInstruction =
                        "Kullanıcı ulaşım seçenekleri hakkında bilgi istiyor.";
                    break;

                case TravelIntent.Currency:
                    intentInstruction =
                        "Kullanıcı döviz kuru bilgisi istiyor.";
                    break;

                case TravelIntent.Itinerary:
                    intentInstruction =
                        "Kullanıcı seyahat planı veya rota hazırlanmasını istiyor.";
                    break;

                case TravelIntent.Attraction:
                    intentInstruction =
                        "Kullanıcı gezilecek yer önerileri istiyor.";
                    break;

                default:
                    intentInstruction =
                        "Kullanıcının seyahatle ilgili sorusuna yardımcı ol.";
                    break;
            }

            var finalPrompt = _travelPromptBuilder.BuildPrompt($"{intentInstruction}\n\nKullanıcının gerçek sorusu:\n{prompt}");

            var result = await _openAIService.GetResponseAsync(finalPrompt);

            result.Intent = intent.ToString();

            return result;
        }

    }
}
