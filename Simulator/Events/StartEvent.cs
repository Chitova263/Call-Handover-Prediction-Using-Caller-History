namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;
    using VerticalHandoverPrediction.CallSession;

    public class StartEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid CallId { get; }
        public Guid MobileTerminalId { get; }
        public Service Service { get; }
        public DateTime Time { get; }

        public StartEvent(Guid eventId, Guid callId, Guid mobileTerminalId, Service service, DateTime time)
        {
            EventId = eventId;
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            Service = service;
            Time = time;
        }
    }

    public class StartEventMap : ClassMap<StartEvent>
    {
        public StartEventMap() => AutoMap();
    }
}