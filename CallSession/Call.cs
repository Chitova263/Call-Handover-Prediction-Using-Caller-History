using MediatR;
using System;
using VerticalHandoverPrediction.CallAdmissionControl;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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

            CAC.StartCACAlgorithm().AdmitCall(call);

            //if call is blocked return null object
            return call;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }
}