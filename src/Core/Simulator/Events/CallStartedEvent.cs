using System;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction
{
    public record CallStartedEvent : IEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; } = DateTime.Now;
        public MobileTerminal MobileTerminal { get; init; }
        public Service Service { get; init; }
    }
}