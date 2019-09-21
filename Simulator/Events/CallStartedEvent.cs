namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;
    using VerticalHandoverPrediction.CallSession;

    public class CallStartedEvent : IEvent
    {
        public Guid EventId { get;  }
        public DateTime Time { get; set; }
        public ICall Call { get; }

        public CallStartedEvent(DateTime time,  ICall call)
        {
            EventId = Guid.NewGuid();
            Call = call;
            Time = time;
        }
    }

    public class CallStartedEventMap: ClassMap<CallStartedEvent>
    {
        public CallStartedEventMap() => AutoMap();
    }
}