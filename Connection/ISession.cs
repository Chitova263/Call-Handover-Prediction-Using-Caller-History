using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public interface ISession
    {
        Guid SessionId { get; }
        Guid RATId { get; }
        DateTime Start { get; }
        DateTime End { get; }
        IList<MobileTerminalState> SessionSequence { get; set; }
        IList<ICall> ActiveCalls { get; set; }

        TimeSpan SessionDuration();
        void SetRATId(Guid id);
        void TerminateSession(IMobileTerminal mobileTerminal, IHetNet hetNet);
        IList<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState);
    }
}