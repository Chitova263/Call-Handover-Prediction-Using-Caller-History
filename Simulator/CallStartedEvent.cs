using System;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallStartedEvent : IEvent
    {
        public Guid EventId { get;  }
        public DateTime Time { get;  }
        public ICall Call { get; }

        public CallStartedEvent(DateTime startTime,  ICall call)
        {
            EventId = Guid.NewGuid();
            Time = startTime;
            Call = call;
        }
    }
}