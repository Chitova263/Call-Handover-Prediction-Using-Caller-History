using System.Collections.Generic;

namespace VerticalHandoverPrediction
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