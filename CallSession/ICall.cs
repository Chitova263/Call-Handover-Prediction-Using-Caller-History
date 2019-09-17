namespace VerticalHandoverPrediction.CallSession
{
    using System;
    public interface ICall
    {
        Guid CallId { get; }
        Guid MobileTerminalId { get; }
        Service Service { get; }
    }
}