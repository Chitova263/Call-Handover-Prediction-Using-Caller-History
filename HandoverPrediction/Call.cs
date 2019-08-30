using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    public class Call 
    {
        public Guid SessionId { get; private set; }
        public MobileTerminal MobileTerminal { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public Service Service { get; private set; }
        public int ActiveCallsInSession { get; set; }
        public List<MobileTerminalState> CallSessionSequence { get; private set; } = new List<MobileTerminalState>();

        private Call(MobileTerminal mobileTerminal, Service service)
        {
            SessionId = Guid.NewGuid();
            StartTime = DateTime.Now;
            MobileTerminal = mobileTerminal;
            Service = service;
            //refactor this 
            if(mobileTerminal.CurrentState.Equals(MobileTerminalState.Idle)) CallSessionSequence.Add(MobileTerminalState.Idle);
        }

        //Factory method to create call
        public static Call InitiateCall(MobileTerminal mobileTerminal, Service service)
        {
            var call = new Call(mobileTerminal, service);
            //Update number of active calls in current session :??? Refactor only update state if call is admitted
            call.ActiveCallsInSession++;
            //Update MT's current state
            call.MobileTerminal.ComputeMobileTerminalCurrentState(service);
            //Update the call session sequence
            call.CallSessionSequence.Add(call.MobileTerminal.CurrentState);
            return call;
        }

        //compute call duration from the state
        //get the bandwidth usage of the type of call,  derived from the state 
        //what RAT is accommodating the call

    }
}

