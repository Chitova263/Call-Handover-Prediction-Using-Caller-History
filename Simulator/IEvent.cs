using System;

namespace VerticalHandoverPrediction.Simulator
{
    public interface IEvent
    {
        public DateTime Time { get;  }
    }
}