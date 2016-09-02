using OpenWeatherMap;
using System;
using System.Threading.Tasks;

namespace TelemetryServices
{

    // Documentation on https://github.com/joancaron/OpenWeatherMap-Api-Net
    // Or: https://gitlab.com/joancaron/OpenWeatherMap-Api-Net.


    //var client = new OpenWeatherMapClient("optionalAppId");
    //var currentWeather = await client.CurrentWeather.GetByName("London");
    //Console.WriteLine(currentWeather.Weather.Value);

    public class OpenWeather
    {
        OpenWeatherMapClient client;
        readonly string locationId;
        DateTime lastMeasureTime = DateTime.MinValue;
        CurrentWeatherResponse currentWeather = null;

        public OpenWeather(string OpenWeatherId, string locationId)
        {
            this.locationId = locationId;
            client = new OpenWeatherMapClient(OpenWeatherId);
        }

        public async Task<CurrentWeatherResponse> GetWeather()
        {
            if (lastMeasureTime.AddMinutes(2) > DateTime.Now) { return currentWeather; }

            currentWeather = await client.CurrentWeather.GetByName(locationId, MetricSystem.Metric);
            lastMeasureTime = DateTime.Now;

            return currentWeather;
        }
    }
}
