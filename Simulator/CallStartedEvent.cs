using System;
using MediatR;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallStartedEvent: INotification
    {
        public Guid EventId { get; }
        public DateTime EndTime { get; }   
        public Guid CallId { get; }
        public Guid MobileTerminalId { get; }
        public Guid SessionId { get; }

        public CallStartedEvent(DateTime endTime, Guid callId, Guid mobileTerminalId, Guid sessionId)
        {
            EventId = Guid.NewGuid();
            EndTime = endTime;
            CallId = callId;
            MobileTerminalId = mobileTerminalId;
            SessionId = sessionId;
        }
    }
}