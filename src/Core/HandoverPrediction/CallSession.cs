using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    //Active Call Sessions are stored in the RAT
    public class CallSession
    {
        public Guid CallSessionId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public List<MobileTerminalState> CallSessionSequence { get; private set; } = new List<MobileTerminalState>();
        private CallSession(DateTime startTime)
        {
            CallSessionId = Guid.NewGuid();
            StartTime = startTime;
        }

        public static CallSession StartSession(DateTime startTime)
        {
            return new CallSession(startTime);
        }

        public List<MobileTerminalState> AddCallToSession(Call call)
        {
            CallSessionSequence.Add(call.MobileTerminal.CurrentState);
            return CallSessionSequence;
        }
    }
}

