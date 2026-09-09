using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.ViewComponents.DefaultViewComponents
{
    public class _DefaultPopularDestinationComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();  
        }
    }
}
