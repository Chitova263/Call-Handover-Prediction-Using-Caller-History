namespace VerticalHandoverPrediction.Mobile
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Simulator;

    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; set; }
        Guid SessionId { get; }
        Modality Modality { get; }
        MobileTerminalState State { get; }
        IReadOnlyCollection<CallLog> CallLogs { get; }
        bool IsActive { get; }

        void AddCallLog(CallLog log);
        void EndCall(CallEndedEvent evt);
        void SetSessionId(Guid sessionId);
        void SetState(MobileTerminalState state);
        MobileTerminalState UpdateMobileTerminalState(ISession session);
        MobileTerminalState UpdateMobileTerminalStateWhenAdmitingNewCallToOngoingSession(IList<ICall> activeCalls);
    }
}