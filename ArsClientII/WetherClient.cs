using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsClientII
{
    public class WetherClient
    {
        private const string BaseUrl2 = "http://api.openweathermap.org/data/2.5/weather";
        private const string ApiKey2 = "4af73d31c590a47216010f82f1a92878";
        private readonly HttpClient _httpClient;

        public WetherClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl2)
            };
        }
        public async Task<string> GetWeatherData(string city)
        {
            string apiUrl = $"{BaseUrl2}?q={city}&appid={ApiKey2}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        string main = data.weather[0].main;
                        string description = data.weather[0].description;
                        double temp = data.main.temp;
                        double temp_min = data.main.temp_min;

                        return $"Main: {main}, Description: {description}, Temp: {temp}, Temp_min: {temp_min}";
                    }
                    else
                    {
                        // Handle the case where the API request was not successful
                        return "Unable to fetch weather data.";
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that occurred during the API request
                    return $"An error occurred: {ex.Message}";
                }
            }
        }

    }
}
