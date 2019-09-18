namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;

    public class CallEndedEvent: IEvent
    {
        public Guid EventId { get; }
        public Guid CallId { get; }
        public Guid MobileTerminalId { get; }
        public DateTime Time { get; set; }

        public CallEndedEvent(Guid callId, Guid mobileTerminalId, DateTime time)
        {
            EventId = Guid.NewGuid();
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            Time = time;
        }
    }

    public class CallEndedEventMap: ClassMap<CallEndedEvent>
    {
        public CallEndedEventMap() => AutoMap();
    }
}