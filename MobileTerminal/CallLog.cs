using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction.Mobile
{
    public class CallLog
    {
        public Guid SessionId { get; set; }
        public Guid RatId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IList<MobileTerminalState> SessionSequence { get; set; }
    }
}