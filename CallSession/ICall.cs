using System;

namespace VerticalHandoverPrediction.CallSession
{
    public interface ICall
    {
        Guid CallId { get; }
        Guid MobileTerminalId { get; }
        Service Service { get; }
    }
}