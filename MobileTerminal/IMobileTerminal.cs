using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.Mobile
{
    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; set; }
        Guid SessionId { get; }
        Modality Modality { get; }
        MobileTerminalState State { get; }
        IList<CallLog> CallHistoryLogs { get; }
        bool Activated { get; set; }

        void EndCall(CallEndedEvent evt);
        void SetSessionId(Guid sessionId);
        void SetState(MobileTerminalState state);
        MobileTerminalState UpdateMobileTerminalState(ISession session);
        MobileTerminalState UpdateMobileTerminalStateWhenAdmitingNewCallToOngoingSession(IList<ICall> activeCalls);
    }
}