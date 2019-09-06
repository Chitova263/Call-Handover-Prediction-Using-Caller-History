using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Events;

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
        void TerminateCall(CallEndedEvent evt);
        void TerminateSession();
        MobileTerminalState UpdateMobileTerminalState(Service service);
    }
}