using System;
using CsvHelper.Configuration.Attributes;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallStartedEvent : IEvent
    {
        public Guid EventId { get;  }
        public DateTime Time { get; set; }
        public ICall Call { get; }

        public CallStartedEvent(DateTime time,  ICall call)
        {
            EventId = Guid.NewGuid();
            Time = time;
            Call = call;
        }
    }
}