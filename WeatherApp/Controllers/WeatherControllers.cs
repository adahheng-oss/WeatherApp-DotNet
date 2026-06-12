using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// WeatherController handles fetching and displaying weather data
// The [Authorize] attribute means the user must be logged in to access any page here
namespace WeatherApp.Controllers
{
    [Authorize] // Redirects to login page if user is not authenticated
    public class WeatherController : Controller
    {
        // HttpClient is used to make calls to external weather APIs
        private readonly HttpClient _httpClient;

        // Your API key from Open-Meteo (free, no key required)
        // We are using Open-Meteo which is completely free with no API key needed
        private const string WeatherApiBase = "https://api.open-meteo.com/v1/forecast";

        // Constructor - ASP.NET injects IHttpClientFactory to create our HttpClient
        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            // Create a named HttpClient for making weather API requests
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: /Weather/Index
        // Main dashboard page - fetches weather data for Omaha, NE by default
        public async Task<IActionResult> Index(string city = "Omaha")
        {
            try
            {
                // Step 1: Convert city name to coordinates using Open-Meteo Geocoding API
                // We need latitude and longitude to fetch weather data
                var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1&language=en&format=json";
                var geoResponse = await _httpClient.GetStringAsync(geoUrl);

                // Parse the JSON response from the geocoding API
                var geoData = JsonSerializer.Deserialize<JsonElement>(geoResponse);

                // Check if the city was found in the geocoding API
                if (!geoData.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                {
                    // City not found - show error on dashboard
                    ViewBag.Error = $"City '{city}' not found. Please try another city.";
                    return View();
                }

                // Extract latitude and longitude from the geocoding response
                var location = results[0];
                var latitude = location.GetProperty("latitude").GetDouble();
                var longitude = location.GetProperty("longitude").GetDouble();
                var locationName = location.GetProperty("name").GetString();
                var country = location.GetProperty("country").GetString();

                // Step 2: Fetch weather data using the coordinates
                // Requesting current temperature, wind speed, humidity, and daily forecasts
                var weatherUrl = $"{WeatherApiBase}?latitude={latitude}&longitude={longitude}" +
                    $"&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code" +
                    $"&daily=temperature_2m_max,temperature_2m_min,weather_code" +
                    $"&temperature_unit=fahrenheit&wind_speed_unit=mph&forecast_days=5&timezone=auto";

                var weatherResponse = await _httpClient.GetStringAsync(weatherUrl);

                // Parse the weather JSON response
                var weatherData = JsonSerializer.Deserialize<JsonElement>(weatherResponse);

                // Step 3: Extract current weather conditions
                var current = weatherData.GetProperty("current");
                var daily = weatherData.GetProperty("daily");

                // Pass weather data to the view using ViewBag
                ViewBag.City = locationName;
                ViewBag.Country = country;
                ViewBag.Temperature = current.GetProperty("temperature_2m").GetDouble();
                ViewBag.Humidity = current.GetProperty("relative_humidity_2m").GetDouble();
                ViewBag.WindSpeed = current.GetProperty("wind_speed_10m").GetDouble();
                ViewBag.WeatherCode = current.GetProperty("weather_code").GetInt32();
                ViewBag.WeatherDescription = GetWeatherDescription(current.GetProperty("weather_code").GetInt32());

                // Pass 5-day forecast data to the view
                ViewBag.ForecastDates = daily.GetProperty("time");
                ViewBag.ForecastMaxTemps = daily.GetProperty("temperature_2m_max");
                ViewBag.ForecastMinTemps = daily.GetProperty("temperature_2m_min");
                ViewBag.ForecastCodes = daily.GetProperty("weather_code");

                // Pass the searched city name back to the view
                ViewBag.SearchedCity = city;

                return View();
            }
            catch (Exception ex)
            {
                // Something went wrong fetching weather data - show error message
                ViewBag.Error = $"Error fetching weather data: {ex.Message}";
                return View();
            }
        }

        // Helper method - converts Open-Meteo weather codes to human readable descriptions
        // Full list of weather codes: https://open-meteo.com/en/docs
        private string GetWeatherDescription(int code)
        {
            return code switch
            {
                0 => "Clear Sky",
                1 => "Mainly Clear",
                2 => "Partly Cloudy",
                3 => "Overcast",
                45 => "Foggy",
                48 => "Icy Fog",
                51 => "Light Drizzle",
                53 => "Moderate Drizzle",
                55 => "Dense Drizzle",
                61 => "Slight Rain",
                63 => "Moderate Rain",
                65 => "Heavy Rain",
                71 => "Slight Snow",
                73 => "Moderate Snow",
                75 => "Heavy Snow",
                80 => "Slight Showers",
                81 => "Moderate Showers",
                82 => "Heavy Showers",
                95 => "Thunderstorm",
                96 => "Thunderstorm with Hail",
                99 => "Thunderstorm with Heavy Hail",
                _ => "Unknown"  // Default case for any unrecognized weather code
            };
        }
    }
}