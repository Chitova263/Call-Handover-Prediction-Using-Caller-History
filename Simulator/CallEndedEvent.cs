using System;
using CsvHelper.Configuration.Attributes;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallEndedEvent: IEvent
    {
        public Guid EventId { get;  set;}
        public Guid CallId { get;  set; }
        public Guid MobileTerminalId { get; set; }
        public DateTime Time { get; set; }

        public CallEndedEvent(Guid callId, Guid mobileTerminalId, DateTime time)
        {
            EventId = Guid.NewGuid();
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            Time = time;
        }
    }
}