
using FlightBooking.AgentServices.OpenAIServices;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.Dtos.AgentDtos;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IOpenAIService _openAIService;
        private readonly ITravelPromptBuilder _travelPromptBuilder;

        public TravelAgentService(IOpenAIService openAIService, ITravelPromptBuilder travelPromptBuilder = null)
        {
            _openAIService = openAIService;
            _travelPromptBuilder = travelPromptBuilder;
        }
        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            var finalPrompt = _travelPromptBuilder.BuildPrompt(prompt);
            return await _openAIService.GetResponseAsync(prompt);
        }
    }
}
