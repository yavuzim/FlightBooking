using FlightBooking.Dtos.RestaurantDtos;

namespace FlightBooking.AgentServices.GooglePlacesServices
{
    public interface IGooglePlaceService
    {
        Task<List<RestaurantDto>> SearchRestaurantsAsync(string city, string searchText);
    }
}
