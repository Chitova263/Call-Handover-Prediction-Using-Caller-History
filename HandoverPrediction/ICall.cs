using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    public interface ICall
    {
        Guid SessionId { get; }
        MobileTerminal MobileTerminal { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        Service Service { get; }
        int ActiveCallsInSession { get; set; }
        List<MobileTerminalState> CallSessionSequence { get; }
    }
}

