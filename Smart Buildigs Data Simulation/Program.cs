using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TelemetryServices;

namespace Smart_Buildigs_Data_Simulation
{
    class Program
    {
        static DeviceClient deviceClient;
        static Telemetry telemetry;
        static OpenWeather openWeather;
        static Config config;

        static void Main(string[] args)
        {
            ProcessArgs(args);
            deviceClient = DeviceClient.CreateFromConnectionString(config.IoTHubDeviceConnectionString);
            openWeather = new OpenWeather(config.OpenWeatherMapApiKey, config.OpenWeatherMapLocationId);
            telemetry = new Telemetry(config.OpenWeatherMapLocationId, Measure, 5);

            Console.WriteLine("Streaming Open Weather Map Sensor Data to Azure IoT Hub.\n\nPress any key to cancel");
            Console.Read();
        }

        static async void Measure()
        {
            try
            {
                var content = new Message(telemetry.ToJson(await openWeather.GetWeather()));
                await deviceClient.SendEventAsync(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static void ProcessArgs(string[] args)
        {
            var configFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (File.Exists(configFilename))
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilename));

                StringBuilder msg = new StringBuilder();
                msg.Append("Config \n");
                msg.Append($"Azure IoT Hub Device Connection String: {config.IoTHubDeviceConnectionString}\n");
                msg.Append($"Open Weather API Key: {config.OpenWeatherMapApiKey}\n");
                msg.Append($"Open Weather Map Location Id: {config.OpenWeatherMapLocationId}\n\n\n");

                Console.WriteLine(msg.ToString());

            }
        }
    }
}
