using System.Threading;
using System.Threading.Tasks;

namespace TelemetryServices
{
    public delegate void MeasureMethod();
    public class Scheduler
    {
        MeasureMethod measureMethod;
        int sampleRateInSeconds = 60;  // defaults to sample every 60 seconds

        public Scheduler(MeasureMethod measureMethod, int sampleRateInSeconds) {
            if (measureMethod == null) { return; }

            this.measureMethod = measureMethod;
            this.sampleRateInSeconds = sampleRateInSeconds;  
            
            Measure();
        }

        async void Measure()
        {
            while (true)
            {
                measureMethod();
                await Task.Delay(sampleRateInSeconds * 1000);
            }
        }
    }
}
