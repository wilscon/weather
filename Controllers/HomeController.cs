using Microsoft.AspNetCore.Mvc;
using Weather.Services;


namespace Weather.Controllers
{
    public class HomeController: Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult Index() { 
        
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city, string state)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state))
            {
                ViewBag.Error = "Please enter both city and state.";
                return View();
            }

            var weatherData = await _weatherService.GetWeatherByCityAsync(city, state);

            if (weatherData == null)
            {
                ViewBag.Error = "No weather data available for this location.";
                return View();
            }

            ViewBag.City = city;
            ViewBag.State = state;
            ViewBag.Temperature = weatherData.Temperature;
            ViewBag.WeatherDescription = weatherData.WeatherDescription;
            ViewBag.image = "images/cloud.png";

            return View();
        }

    }
}
