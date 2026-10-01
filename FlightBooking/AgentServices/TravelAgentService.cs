
using FlightBooking.AgentServices.OpenAIServices;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IOpenAIService _openAIService;

        public TravelAgentService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        //public async Task<string> GetRestaurantRecommendationAsync(string cityName)
        //{
        //    var prompt = $"{cityName} şehrine giden bir turist için 5 restoran öner.";
        //    return await _openAIService.GetResponseAsync(prompt);
        //}
        public async Task<string> AskAgentAsync(string prompt)
        {
            return await _openAIService.GetResponseAsync(prompt);
        }
    }
}
