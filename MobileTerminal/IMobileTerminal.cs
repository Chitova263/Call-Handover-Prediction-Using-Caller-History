using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; set; }
        List<CallSession> CallHistoryLog { get; set; }
        MobileTerminalState CurrentState { get; set; }
        CallSession CurrentSession { get; set; }

        bool IsOnActiveSession();
        MobileTerminalState SetMobileTerminalCurrentState(Service service);
    }
}