namespace VerticalHandoverPrediction.CallSession
{
    using System;
    using System.Collections.Generic;
    using VerticalHandoverPrediction.Mobile;

    public interface ISession
    {
        Guid SessionId { get; }
        Guid RatId { get; }
        DateTime Start { get; }
        DateTime End { get; }
        IReadOnlyCollection<MobileTerminalState> SessionSequence { get; }
        IReadOnlyCollection<ICall> ActiveCalls { get; }

        void AddToActiveCalls(ICall call);
        void AddToSessionSequence(MobileTerminalState state);
        void RemoveFromActiveCalls(ICall call);
        void RemoveFromSessionSequence(MobileTerminalState state);
        void SetEndTime(DateTime end);
        void SetRatId(Guid ratId);
    }
}