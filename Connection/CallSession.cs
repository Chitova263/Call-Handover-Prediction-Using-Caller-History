using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VerticalHandoverPrediction
{
    [JsonObject(IsReference = true)]
    public class CallSession : ICallSession
    {
        public Guid CallSessionId { get; private set; }
        public Guid RATId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public IList<MobileTerminalState> CallSessionSequence { get; set; } = new List<MobileTerminalState>() { MobileTerminalState.Idle };
        public IList<ICall> ActiveCalls { get; set; } = new List<ICall>();
        private CallSession(IMobileTerminal mobileTerminal, Guid ratId)
        {
            if (mobileTerminal is null)
            {
                throw new ArgumentNullException(nameof(mobileTerminal));
            }

            RATId = ratId;
            CallSessionId = Guid.NewGuid();
            CallSessionSequence.Add(mobileTerminal.CurrentState);
            Start = DateTime.Now;
        }

        public static CallSession InitiateSession(IMobileTerminal mobileTerminal, Guid ratId)
        {
            return new CallSession(mobileTerminal, ratId);
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

        public void SetRATId(Guid id)
        {
            RATId = id;
        }
    }
}