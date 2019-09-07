using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.Mobile
{
    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; }
        Guid SessionId { get; }
        Modality Modality { get; }
        MobileTerminalState State { get; }
        IList<CallLog> CallHistoryLogs { get; }

        void EndCall(CallEndedEvent evt);
        void SetSessionId(Guid sessionId);
        void SetState(MobileTerminalState state);
    }
}