using MediatR;
using System;
using Microsoft.Extensions.DependencyInjection;
using VerticalHandoverPrediction.Simulator;

namespace VerticalHandoverPrediction.CallSession
{

    public class Call : ICall, INotification
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
           
            //var isCallAdmitted = CAC.StartCACAlgorithm().AdmitCall(call);

            //Publish event
            
            //var mediator = DIContainer._Container.Container.GetRequiredService<IMediator>();
            //var @event = new CallStartedEvent(DateTime.Now.AddMinutes(1),
                                                call.CallId,
                                                call.MobileTerminalId,
                                                call.SessionId);
            //mediator.Publish(@event).Wait();
            
            return call;  
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }
}