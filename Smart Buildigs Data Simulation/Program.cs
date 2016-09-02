using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
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
            openWeather = new OpenWeather(config.OpenWeatherId, config.OpenWeatherlocationId);
            telemetry = new Telemetry(config.OpenWeatherlocationId, Measure, 5);

            Console.WriteLine("Press any key to cancel");
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
            }
        }
    }
}
