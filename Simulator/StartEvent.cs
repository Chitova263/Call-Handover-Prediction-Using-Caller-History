using System;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Simulator
{
    public class StartEvent: IEvent
    {
        public Guid EventId { get; set; }
        public Guid CallId { get; set; }
        public Guid MobileTerminalId { get; set; }
        public Service Service { get; set; }
        public DateTime Time { get; set ; }
    }

    public class StartEventMap : ClassMap<StartEvent>
    {
        public StartEventMap()
        {
            AutoMap();
        }
    }
}