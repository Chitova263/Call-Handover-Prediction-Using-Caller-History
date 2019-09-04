using System;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallInitiatedEvent : IEvent
    {
        public Guid EventId { get; }
        public DateTime Time { get; }

        public CallInitiatedEvent(DateTime time)
        {
            EventId = Guid.NewGuid();
            Time = time;
        }
    }
}