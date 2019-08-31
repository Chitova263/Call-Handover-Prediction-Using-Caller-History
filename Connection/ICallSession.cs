using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface ICallSession
    {
        Guid CallSessionId { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        List<MobileTerminalState> CallSessionSequence { get; set; }
        List<Call> ActiveCalls { get; set; }

        TimeSpan SessionDuration();
        void TerminateSession(MobileTerminal mobileTerminal);
        List<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState);
    }
}