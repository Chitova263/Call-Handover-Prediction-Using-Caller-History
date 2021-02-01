using System;

namespace VerticalHandoverPrediction
{
    public interface IEvent
    {
        public DateTime Timestamp { get; }
    }
}