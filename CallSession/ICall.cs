using System;

namespace VerticalHandoverPrediction.CallSession
{
    public interface ICall
    {
        Guid CallId { get; }
        Guid SessionId { get; }
        Guid MobileTerminalId { get; }
        Service Service { get; }

        void SetSessionId(Guid sessionId);
    }
}