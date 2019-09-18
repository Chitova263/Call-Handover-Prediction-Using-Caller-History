namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;
    using CsvHelper.Configuration;
    public class EndEvent : IEvent
    {
        public Guid EventId { get; set; }
        public Guid CallId { get; set; }
        public Guid MobileTerminalId { get; set; }
        public DateTime Time { get; set; }

    }

    public class EndEventMap : ClassMap<EndEvent>
    {
        public EndEventMap() => AutoMap();
    }
}