using System;
using System.Collections.Generic;
using System.Linq;

namespace  VerticalHandoverPrediction
{
    public static class NetworkExtensions
    {
        public static int ComputeRequiredCapacity(this ISession session)
        {
            var utlizedCapacity = 0;
            foreach (var call in session.ActiveCalls)
            {
                switch (call.Service)
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
            }
            return utlizedCapacity;
        }

        public static int ComputeRequiredCapacity(this ICall call)
        {
            var utlizedCapacity = 0;
            switch (call.Service)
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

        public static MobileTerminalState GetState(this Service service)
        {
            MobileTerminalState state = default(MobileTerminalState);
            switch (service)
            {
                case Service.Voice:
                    state = MobileTerminalState.Voice;
                    break;
                case Service.Video:
                    state = MobileTerminalState.Video;
                    break;
                case Service.Data:
                    state = MobileTerminalState.Data;
                    break;
            }
            return state;
        }
        
    }
}