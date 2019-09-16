using System;

namespace VerticalHandoverPrediction.CallSession
{

    public class Call : ICall
    {
        public Guid CallId { get; private set; }
        public Guid MobileTerminalId { get; private set; }
        public Service Service { get; private set; }

        private Call(Guid mobileTerminalId, Service service)
        {
            CallId = Guid.NewGuid();
            MobileTerminalId = mobileTerminalId;
            Service = service;
        }

        public Call(Guid mobileTerminalId, Service service, Guid callId)
        {
            MobileTerminalId = mobileTerminalId;
            Service = service;
            CallId = callId;
        }

        public static Call StartCall(Guid mobileTerminalId, Service service)
        {
            return new Call(mobileTerminalId, service);
        }
    }
}