using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace VerticalHandoverPrediction
{

    [JsonObject(IsReference = true)]
    public class Session : ISession
    {
        public Guid SessionId { get; private set; }
        public Guid RATId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public IList<MobileTerminalState> SessionSequence { get; set; } = new List<MobileTerminalState>() { MobileTerminalState.Idle };
        public IList<ICall> ActiveCalls { get; set; } = new List<ICall>();
        private Session(ICall call, Guid ratId)
        {
            RATId = ratId;
            SessionId = Guid.NewGuid();
            Start = DateTime.Now;
        }

        //Static Factory Method
        public static Session InitiateSession(ICall call, Guid ratId)
        {
            var session = new Session(call, ratId);
            //set SessionId on mobileTerminal
            call.MobileTerminal.SetSessionId(session.SessionId);
            //update MT RATId
            call.MobileTerminal.SetRATId(ratId);
            //Update MT state
            call.MobileTerminal.UpdateMobileTerminalState(call.Service);
            //add call to active call list
            session.ActiveCalls.Add(call);
            //update the call sequence
            session.SessionSequence.Add(call.MobileTerminal.State);
            return session;
        }

        public IList<MobileTerminalState> UpdateCallSessionSequence(MobileTerminalState mobileTerminalState)
        {
            SessionSequence.Add(mobileTerminalState);
            return SessionSequence;
        }

        //Make HetNet A singleton
        public void TerminateSession(IMobileTerminal mobileTerminal, IHetNet hetNet)
        {
            //Set The End Time
            this.End = DateTime.Now;
            //Update MobileTerminal State
            mobileTerminal.SetMobileTerminalState(MobileTerminalState.Idle);
            //Update session sequence
            this.SessionSequence.Add(mobileTerminal.State);
            //Update CallLogHistory
            mobileTerminal.UpdateCallHistoryLog(this);

            /* Dismiss From RAT */

            // Find the RAT accommodating session from the HetNet
            var rat = hetNet.RATs
                .FirstOrDefault(x => x.RATId == mobileTerminal.RATId);

            // Free Up Resources from the RAT
            rat.TerminateSession(mobileTerminal.SessionId);
        }

        public TimeSpan SessionDuration()
        {
            var duration = End.Subtract(Start);
            return duration;
        }

        public void SetRATId(Guid id) => RATId = id;
    }
}