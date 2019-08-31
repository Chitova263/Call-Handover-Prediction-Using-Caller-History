using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public class CallSession : ICallSession
    {
        public Guid CallSessionId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public IList<MobileTerminalState> CallSessionSequence { get; set; } = new List<MobileTerminalState>() { MobileTerminalState.Idle };
        public IList<ICall> ActiveCalls { get; set; } = new List<ICall>();
        private CallSession(IMobileTerminal mobileTerminal)
        {
            CallSessionId = Guid.NewGuid();
            CallSessionSequence.Add(mobileTerminal.CurrentState);
            Start = DateTime.Now;
        }

        public static CallSession InitiateSession(IMobileTerminal mobileTerminal)
        {
            return new CallSession(mobileTerminal);
        }

        public IList<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState)
        {
            CallSessionSequence.Add(mobileTerminalState);
            return CallSessionSequence;
        }

        public void TerminateSession(IMobileTerminal mobileTerminal)
        {
            //set mobile terminal current state to idle
            mobileTerminal.CurrentState = MobileTerminalState.Idle;
            //removes all active calls from the list
            this.ActiveCalls.Clear();
            //set end time of call
            this.End = DateTime.Now;
            //update the call session sequence
            this.CallSessionSequence.Add(mobileTerminal.CurrentState);
            //update mobile terminal call history
            mobileTerminal.CallHistoryLog.Add(this);
        }

        public TimeSpan SessionDuration()
        {
            var duration = End.Subtract(Start);
            return duration;
        }
    }
}