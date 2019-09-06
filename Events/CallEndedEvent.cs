using System;
using MediatR;

namespace VerticalHandoverPrediction.Events
{
    public class CallEndedEvent: INotification, IEvent
    {
        public Guid EventId { get;  }
        public Guid CallId { get;  }
        public Guid MobileTerminalId { get;  }
        public DateTime Time { get;  }

        public CallEndedEvent(Guid callId, Guid mobileTerminalId)
        {
            EventId = Guid.NewGuid();
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
        }
    }
}