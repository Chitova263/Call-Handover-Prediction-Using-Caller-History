using CsvHelper.Configuration;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.Utils
{
    public class CallStartedEventMap: ClassMap<CallStartedEvent>
    {
        public CallStartedEventMap()
        {
            AutoMap();
        }
    }
}