using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface ICallSession
    {
        Guid CallSessionId { get; }
        DateTime Start { get; }
        DateTime End { get; }
        IList<MobileTerminalState> CallSessionSequence { get; set; }
        IList<ICall> ActiveCalls { get; set; }

        TimeSpan SessionDuration();
        void TerminateSession(IMobileTerminal mobileTerminal);
        IList<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState);
    }
}