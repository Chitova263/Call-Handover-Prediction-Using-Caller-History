using CsvHelper.Configuration;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.Utils
{
    public class CallEndedEventMap: ClassMap<CallEndedEvent>
    {
        public CallEndedEventMap()
        {
           AutoMap();
        }
    }
}