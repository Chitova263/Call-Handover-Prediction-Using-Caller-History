namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;
    public class EndEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid CallId { get; }
        public Guid MobileTerminalId { get; }
        public DateTime Time { get; }

        public EndEvent(Guid eventId, Guid callId, Guid mobileTerminalId, DateTime time)
        {
            EventId = eventId;
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            Time = time;
        }
    }

    public class EndEventMap : ClassMap<EndEvent>
    {
        public EndEventMap() => AutoMap();
    }
}