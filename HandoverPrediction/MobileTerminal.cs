using System;
using System.Collections.Generic;

namespace HandoverPrediction
{
    public class MobileTerminal
    {
        public Guid MobileTerminalId { get; }
        public List<Call> CallHistoryLog { get; }
        


        public MobileTerminal(Guid callerId) => CallerId = callerId;
    }
}

