using Weather.Models;

namespace Weather.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherByCityAsync(string city, string state);

    }
}
                                                                    