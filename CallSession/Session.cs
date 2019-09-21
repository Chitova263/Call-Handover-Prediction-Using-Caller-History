namespace VerticalHandoverPrediction.CallSession
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.Mobile;

    public class Session : ISession
    {
        public Guid SessionId { get; private set; }
        public Guid RatId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        private readonly List<MobileTerminalState> _sessionSequence;
        public IReadOnlyCollection<MobileTerminalState> SessionSequence => _sessionSequence;
        private readonly List<ICall> _activeCalls;
        public IReadOnlyCollection<ICall> ActiveCalls => _activeCalls;

        public Session(Guid ratId, DateTime start)
        {
            if (ratId == Guid.Empty)
            {
                throw new VerticalHandoverPredictionException($"{nameof(ratId)} is invalid");
            }

            SessionId = Guid.NewGuid();
            RatId = ratId;
            Start = start;
            _sessionSequence = new List<MobileTerminalState> { MobileTerminalState.Idle };
            _activeCalls = new List<ICall>();
        }

        public void AddToActiveCalls(ICall call)
        {
            if (call == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");
            }
            _activeCalls.Add(call);
        }

        public void RemoveFromActiveCalls(ICall call)
        {
            if (call == null)
            {
                throw new VerticalHandoverPredictionException($"{nameof(call)} is invalid");
            }
            _activeCalls.Remove(call);
        }

        public void AddToSessionSequence(MobileTerminalState state) => _sessionSequence.Add(state);

        public void RemoveFromSessionSequence(MobileTerminalState state) => _sessionSequence.Remove(state);
        public void SetRatId(Guid ratId) => RatId = ratId;

        public void SetEndTime(DateTime end) => End = end;
    }
}