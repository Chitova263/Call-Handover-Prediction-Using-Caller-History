using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface IMobileTerminal
    {
        Guid MobileTerminalId { get; }
        Guid SessionId { get; }
        Guid RATId { get; }
        IList<ISession> CallHistoryLog { get; }
        MobileTerminalState State { get; }
        MobileTerminalModality Mode { get; }

        void SetMobileTerminalState(MobileTerminalState state);
        void SetRATId(Guid ratId);
        void SetSessionId(Guid sessionId);
        void TerminateSession(IHetNet hetNet);
        void UpdateCallHistoryLog(ISession session);
        MobileTerminalState UpdateMobileTerminalState(Service service);
    }
}