using VerticalHandoverPrediction.Models;

namespace VerticalHandoverPrediction.Simulator.Events
{
    public record CallEndedEvent : IEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public Guid CallStartedEventId { get; init; }
        public MobileTerminal MobileTerminal { get; init; }
        public DateTime Timestamp { get; init; }
    }
}