using System;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallEndedEvent : IEvent
    {
        public Guid EventId { get; }
        public DateTime Time { get; }

        public CallEndedEvent(DateTime time)
        {
            EventId = Guid.NewGuid();
            Time = time;
        }
    }
}