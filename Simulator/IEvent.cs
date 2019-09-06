using System;

namespace VerticalHandoverPrediction.Events
{
    public interface IEvent
    {
        public DateTime Time { get;  }
    }
}