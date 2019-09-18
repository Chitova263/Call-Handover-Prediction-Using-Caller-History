namespace VerticalHandoverPrediction.Simulator.Events
{
    using System;

    public interface IEvent
    {
        public DateTime Time { get; set; }
    }
}