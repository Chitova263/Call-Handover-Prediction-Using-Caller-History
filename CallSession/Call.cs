using System;
using Serilog;
using VerticalHandoverPrediction.CallAdmissionControl;

namespace VerticalHandoverPrediction.CallSession
{

    public class Call : ICall
    {
        public Guid CallId { get; private set; }
        public Guid SessionId { get; private set; } //set after call is admitted
        public Guid MobileTerminalId { get; private set; }
        public Service Service { get; private set; }

        private Call(Guid mobileTerminalId, Service service)
        {
            CallId = Guid.NewGuid();
            MobileTerminalId = mobileTerminalId;
            Service = service;
        }

        public static Call StartCall(Guid mobileTerminalId, Service service)
        {
            var call = new Call(mobileTerminalId, service);

            //Perfom CAC Algorithm on call object
            Log.Information("Calling...............");
            
            NonPredictiveCAC.StartCACAlgorithm().AdmitCall(call);

            //if call is blocked return null object
            return call;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
        }

        public void TerminateCall()
        {

        }
    }
}