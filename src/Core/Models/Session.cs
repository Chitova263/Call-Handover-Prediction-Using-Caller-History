using VerticalHandoverPrediction.Extensions;

namespace VerticalHandoverPrediction.Models
{
    public sealed class Session
    {
        public Guid SessionId { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public LinkedList<MobileTerminalState> MobileTerminalStateSequence { get; }
        public Dictionary<Guid, Call> ActiveCalls { get;  }

        private Session(DateTime startTime, Call call, LinkedList<MobileTerminalState> mobileTerminalStateSequence)
        {
            SessionId = Guid.NewGuid();
            StartTime = startTime;
            MobileTerminalStateSequence = mobileTerminalStateSequence;
            ActiveCalls = new Dictionary<Guid, Call>();
            ActiveCalls.Add(call.CallId, call);
        }

        public static Session CreateSession(DateTime startTime, Call call)
        {
            var mobileTerminalStateSequence = new LinkedList<MobileTerminalState>();
            mobileTerminalStateSequence.AddLast(MobileTerminalState.Idle);

            //Based on the initial call type get the initial state
            MobileTerminalState mobileTerminalState;
            switch (call.Service)
            {
                case Service.Voice:
                    mobileTerminalState = MobileTerminalState.Voice;
                    break;
                case Service.Data:
                    mobileTerminalState = MobileTerminalState.Data;
                    break;
                case Service.Video:
                    mobileTerminalState = MobileTerminalState.Video;
                    break;
                default:
                    throw new Exception();
            }

            mobileTerminalStateSequence.AddLast(mobileTerminalState);

            return new Session(startTime, call, mobileTerminalStateSequence);
        }

        public void UpdateMobileTerminalStateSequence(Service incomingCall)
        {
            var nextState = MobileTerminalStateSequence.Last.Value | incomingCall.DeriveMobileTerminalState();
            MobileTerminalStateSequence.AddLast(nextState);
        }
    }
}