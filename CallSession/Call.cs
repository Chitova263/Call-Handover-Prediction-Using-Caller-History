using MediatR;
using System;
using VerticalHandoverPrediction.CallAdmissionControl;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VerticalHandoverPrediction.Simulator;

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
           
            var isCallAdmitted = CAC.StartCACAlgorithm().AdmitCall(call);

            //Publish event
            if(isCallAdmitted)
            {
                var mediator = DIContainer._Container.Container.GetRequiredService<IMediator>();
                var @event = new CallStartedEvent(DateTime.Now.AddMinutes(1),
                                                  call.CallId,
                                                  call.MobileTerminalId,
                                                  call.SessionId);
                mediator.Publish(@event).Wait();
                return call;
            }
            
            //Call is blocked
            return null;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }
}