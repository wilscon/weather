using Weather.Models;
using Newtonsoft.Json;

namespace Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration) { 
            _httpClient = httpClient;
            _configuration = configuration;
        
        }

        public async Task<WeatherData> GetWeatherByCityAsync(string city, string state)
        {
            var apiKey = _configuration["WeatherApi:ApiKey"];
            var baseUrl = _configuration["WeatherApi:BaseUrl"];

            // Concatenate city, state, and "US" (for the United States)
            var location = $"{city},{state},US";

            var url = $"{baseUrl}?location={location}&fields=temperature,weatherCode&timesteps=1h&units=imperial&apikey={apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(json);

                // Get the current time in UTC (since API times are in UTC)
                var currentTime = DateTime.UtcNow;

                // Find the interval with the closest startTime to now
                var latestTimeline = weatherResponse?.Data?.Timelines?.FirstOrDefault();
                var currentInterval = latestTimeline?.Intervals
                    ?.OrderBy(interval => Math.Abs((interval.StartTime - currentTime).Ticks))
                    .FirstOrDefault();

                if (currentInterval != null)
                {
                    // Map the weatherCode to a human-readable description
                    var weatherDescription = MapWeatherCodeToDescription(currentInterval.Values.WeatherCode);

                    return new WeatherData
                    {
                        Temperature = currentInterval.Values.Temperature,
                        WeatherDescription = weatherDescription
                    };
                }
            }

            return null; // In case of failure
        }

        private string MapWeatherCodeToDescription(int weatherCode)
        {
            return weatherCode switch
            {
                1000 => "Clear",
                1001 => "Cloudy",
                1100 => "Mostly Clear",
                1101 => "Partly Cloudy",
                1102 => "Mostly Cloudy",
                2000 => "Fog",
                2100 => "Light Fog",
                3000 => "Light Wind",
                3001 => "Wind",
                3002 => "Strong Wind",
                4000 => "Drizzle",
                4001 => "Rain",
                4200 => "Light Rain",
                4201 => "Heavy Rain",
                5000 => "Snow",
                5001 => "Flurries",
                5100 => "Light Snow",
                5101 => "Heavy Snow",
                6000 => "Freezing Drizzle",
                6001 => "Freezing Rain",
                6200 => "Light Freezing Rain",
                6201 => "Heavy Freezing Rain",
                _ => "Unknown",
            };
        }
    }
}
