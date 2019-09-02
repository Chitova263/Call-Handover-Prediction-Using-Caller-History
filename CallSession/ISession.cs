using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.CallSession
{
    public interface ISession
    {
        Guid SessionId { get; }
        Guid RatId { get; }
        DateTime Start { get; }
        DateTime End { get; }
        IList<MobileTerminalState> SessionSequence { get; set; }
        IList<ICall> ActiveCalls { get; }

        void SetRatId(Guid ratId);
        void SetEndTime(DateTime now);
    }
}