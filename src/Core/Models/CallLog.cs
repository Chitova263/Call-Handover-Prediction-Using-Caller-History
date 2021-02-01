using System;

namespace VerticalHandoverPrediction
{
    public record CallLog
    {
        public Guid UserId { get; init; }
        public Guid SessionId { get; init; }
        public DateTime StartTime { get; init; }
        public TimeSpan Duration { get; init; }
        public string SessionSequence { get; init; }
    }
}