using Newtonsoft.Json;
using OpenWeatherMap;
using System;
using System.Text;

namespace TelemetryServices
{

    public class Telemetry : Scheduler
    {
        static int msgCount = 0;

        public string Geo { get; set; }
        public string Celsius { get; set; }
        public string CelsiusMax { get; set; }
        public string CelsiusMin { get; set; }
        public string Humidity { get; set; }
        public string HPa { get; set; }
        public string Cloud { get; set; }
        public string WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public int Id { get; set; }


        public Telemetry(string geo, MeasureMethod measureMethod = null, int sampleRateInSeconds = 60) : base(measureMethod, sampleRateInSeconds) {
            this.Geo = geo;
        }

        public byte[] ToJson(CurrentWeatherResponse weather) {
            Celsius = RoundMeasurement(weather.Temperature.Value, 2).ToString();
            CelsiusMin = RoundMeasurement(weather.Temperature.Min, 2).ToString();
            CelsiusMax = RoundMeasurement(weather.Temperature.Max, 2).ToString();
            Cloud = RoundMeasurement(weather.Clouds.Value, 2).ToString();
            HPa = RoundMeasurement(weather.Pressure.Value, 0).ToString();
            Humidity = RoundMeasurement(weather.Humidity.Value, 2).ToString();
            WindSpeed = RoundMeasurement(weather.Wind.Speed.Value, 0).ToString();
            WindDirection = RoundMeasurement(weather.Wind.Direction.Value, 0).ToString();
            Id = ++msgCount;
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        private string RoundMeasurement(double value, int places) {
            return Math.Round(value, places).ToString();
        }
    }
}
