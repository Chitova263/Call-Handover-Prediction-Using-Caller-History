using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.CallSession
{
    public class Session : ISession
    {
        public Guid SessionId { get; private set; }
        public Guid RatId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public IList<MobileTerminalState> SessionSequence { get; set; }
        public IList<ICall> ActiveCalls { get; private set; }

        private Session(Guid ratId)
        {
            SessionId = Guid.NewGuid();
            RatId = ratId;
            Start = DateTime.Now;
            SessionSequence = new List<MobileTerminalState> { MobileTerminalState.Idle };
            ActiveCalls = new List<ICall>();
        }

        public static Session StartSession(Guid ratId)
        {
            var session = new Session(ratId);
            //Build Session Object
            return session;
        }

        public void SetRatId(Guid ratId) => RatId = ratId;

        public void SetEndTime(DateTime end) => End = end;
    }
}