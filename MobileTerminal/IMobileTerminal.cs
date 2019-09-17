namespace VerticalHandoverPrediction.Mobile
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Simulator.Events;

    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; }
        Guid SessionId { get; }
        MobileTerminalState State { get; }
        IReadOnlyCollection<CallLog> CallLogs { get; }
        bool IsActive { get; }

        void AddCallLog(CallLog log);
        void EndCall(CallEndedEvent evt);
        void SetActive(bool isActive);
        void SetSessionId(Guid sessionId);
        void SetState(MobileTerminalState state);
        MobileTerminalState UpdateMobileTerminalState(ISession session);
        MobileTerminalState UpdateMobileTerminalStateWhenAdmitingNewCallToOngoingSession(IEnumerable<ICall> activeCalls);
    }
}