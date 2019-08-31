using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{

    public class CallSession : ICallSession
    {
        public Guid CallSessionId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<MobileTerminalState> CallSessionSequence { get; set; } = new List<MobileTerminalState>() { MobileTerminalState.Idle };
        public List<Call> ActiveCalls { get; set; } = new List<Call>();
        private CallSession(MobileTerminal mobileTerminal)
        {
            CallSessionId = Guid.NewGuid();
            CallSessionSequence.Add(mobileTerminal.CurrentState);
            Start = DateTime.Now;
        }

        public static CallSession InitiateSession(MobileTerminal mobileTerminal)
        {
            return new CallSession(mobileTerminal);
        }

        public List<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState)
        {
            CallSessionSequence.Add(mobileTerminalState);
            return CallSessionSequence;
        }

        public void TerminateSession(MobileTerminal mobileTerminal)
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