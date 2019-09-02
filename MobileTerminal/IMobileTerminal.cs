using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.Mobile
{
    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; }
        Guid SessionId { get; }
        Modality Modality { get; }
        MobileTerminalState State { get; }
        IList<CallLog> CallHistoryLogs { get; }

        MobileTerminalState ChangeStateTo(MobileTerminalState state);
        void SetSessionId(Guid sessionId);
        void TerminateCall(Guid callId);
        void TerminateSession();
        MobileTerminalState UpdateMobileTerminalState(Service service);
    }
}