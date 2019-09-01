using System;

namespace VerticalHandoverPrediction
{
    public interface ICall
    {
        Guid CallId { get; }
        IMobileTerminal MobileTerminal { get; }
        Service Service { get; }

        void TerminateCall(ICall call);
    }
}