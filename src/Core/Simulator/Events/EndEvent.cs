using System;
using CsvHelper.Configuration;

namespace VerticalHandoverPrediction
{
    public record EndEvent
    {
        public Guid EventId { get; set; }
        public Guid CallId { get; set; }
        public Guid MobileTerminalId { get; set; }
        public DateTime Time { get; set; }

    }
}