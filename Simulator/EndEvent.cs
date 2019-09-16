using System;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace VerticalHandoverPrediction.Simulator
{
    public class EndEvent: IEvent
    {
        public Guid EventId { get;  set;}
        public Guid CallId { get;  set; }
        public Guid MobileTerminalId { get; set; }
        public DateTime Time { get; set; }
    }

    public class EndEventMap : ClassMap<EndEvent>
    {
        public EndEventMap()
        {
            AutoMap();
        }
    }
}