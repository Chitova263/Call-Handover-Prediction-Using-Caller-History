using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.Simulator
{
    public class DateTimeComparer : IComparer<IEvent>
    {
        public int Compare(IEvent x, IEvent y)
        {
            if (x.Timestamp > y.Timestamp)
                return 1;
            if (x.Timestamp < y.Timestamp)
                return -1;
            else
                return 0;
        }
    }
}