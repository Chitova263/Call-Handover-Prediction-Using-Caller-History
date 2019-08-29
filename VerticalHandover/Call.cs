using System;
using System.Collections.Generic;

namespace Handoverprediction.VerticalHandover
{
    public class Call
    {
        public Guid SessionId { get;  }
        public Guid UserId { get;  }
        public DateTime StartTime { get;  }
        public DateTime EndTime { get;  }
        public int BasebandUnits { get;  }
        public List<MobileTerminalState> SessionHistory { get;  }
        public MobileTerminalState State { get; set; }

        public Call(Guid userId, MobileTerminalState state)
        {
            StartTime = DateTime.UtcNow;
            SessionId = Guid.NewGuid();
            UserId = userId;
            State = state;
        }
    }
}
