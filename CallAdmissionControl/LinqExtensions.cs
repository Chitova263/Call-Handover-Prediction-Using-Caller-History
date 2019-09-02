using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using System.Linq;
using System;

namespace VerticalHandoverPrediction.CallAdmissionControl
{
    public static class LinqExtensions
    {
        public static ISession FindSessionWithId(this IEnumerable<ISession> sessions, Guid id)
        {
            var session = sessions.FirstOrDefault(x => x.SessionId == id );
            return session;
        }

        public static int ComputeRequiredCapacity(this Service service)
        {
            var utlizedCapacity = 0;
            switch (service)
            {
                case Service.Voice:
                    utlizedCapacity += 1;
                    break;
                case Service.Data:
                    utlizedCapacity += 2;
                    break;
                case Service.Video:
                    utlizedCapacity += 2;
                    break;
            }
            return utlizedCapacity;
        }
    }
}