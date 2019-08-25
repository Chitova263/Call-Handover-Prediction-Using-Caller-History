using System;
using System.Collections.Generic;

namespace Handoverprediction.VerticalHandover
{
    public class Call
    {
        public Guid SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Duration { get; set; }
        public List<MobileTerminalStates> SessionPath { get; set; }
    }
}
