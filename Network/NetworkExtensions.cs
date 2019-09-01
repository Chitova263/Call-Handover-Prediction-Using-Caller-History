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
        

    }
}