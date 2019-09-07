using System;
using MediatR;

namespace VerticalHandoverPrediction.Events
{
    public class CallEndedEvent: IEvent
    {
        public Guid EventId { get;  }
        public Guid CallId { get;  }
        public Guid MobileTerminalId { get;  }
        public DateTime Time { get;  }

        public CallEndedEvent(Guid callId, Guid mobileTerminalId, DateTime time)
        {
            EventId = Guid.NewGuid();
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            Time = time;
        }
    }
}