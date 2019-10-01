namespace VerticalHandoverPrediction.CallSession
{
    using System;

    public class Call : ICall
    {
        public Guid CallId { get; private set; }
        public Guid MobileTerminalId { get; private set; }
        public Service Service { get; private set; }

        public Call(Guid mobileTerminalId, Service service)
        {
            if(mobileTerminalId == Guid.Empty)
                throw new VerticalHandoverPredictionException($"{nameof(mobileTerminalId)} is empty");

            CallId = Guid.NewGuid();
            MobileTerminalId = mobileTerminalId;
            Service = service;
        }

        public Call(Guid mobileTerminalId, Service service, Guid callId) : this(mobileTerminalId, service)
        {
            if(callId == Guid.Empty)
            {
                throw new VerticalHandoverPredictionException($"{nameof(callId)} is invalid");
            }

            CallId = callId;
        }
    }
}