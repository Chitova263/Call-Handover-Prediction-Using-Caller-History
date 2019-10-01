namespace VerticalHandoverPrediction.Simulator
{
    using System.Collections.Generic;
    using VerticalHandoverPrediction.Simulator.Events;

    public class DateTimeComparer : IComparer<IEvent>
    {
        public int Compare(IEvent x, IEvent y)
        {
            if (x.Time > y.Time)
                return 1;
            if (x.Time < y.Time)
                return -1;
            else
                return 0;
        }
    }
}