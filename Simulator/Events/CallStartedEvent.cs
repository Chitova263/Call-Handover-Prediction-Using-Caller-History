namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;
    using VerticalHandoverPrediction.CallSession;

    public class CallStartedEvent : IEvent
    {
        public Guid EventId { get;  }
        public DateTime Time { get; }
        public ICall Call { get; }

        public CallStartedEvent(DateTime time,  ICall call)
        {
            EventId = Guid.NewGuid();
            Time = time;
            Call = call;
        }
    }

    public class CallStartedEventMap: ClassMap<CallStartedEvent>
    {
        public CallStartedEventMap() => AutoMap();
    }
}