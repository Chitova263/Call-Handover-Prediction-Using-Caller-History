using System;
using System.Collections.Generic;
using System.Linq;
using Medallion.Collections;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Commands;
using VerticalHandoverPrediction.Events;
using VerticalHandoverPrediction.Network;

namespace VerticalHandoverPrediction.Simulator
{
    public class DateTimeComparer : IComparer<IEvent>
    {
        public int Compare(IEvent x, IEvent y)
        {
            if (x.Time > y.Time)
                return 1;
            if (x.Time < y.Time)
                return -1;
            else
                return 0;
        }
    }

    public sealed class NetworkSimulator 
    {
        private static NetworkSimulator instance = null;
        private static readonly object padlock = new object();
        public PriorityQueue<IEvent> EventQueue { get; set; }
        private NetworkSimulator()
        {
            EventQueue = new PriorityQueue<IEvent>(new DateTimeComparer());
        }

        public static NetworkSimulator _NetworkSimulator
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NetworkSimulator();
                    }
                    return instance;
                }
            }
        }

        public void Run(int n)
        {
            Random rnd = new Random();
            var _mediator = DIContainer._Container.Container.GetRequiredService<IMediator>();
            var services = new List<Service>{Service.Data, Service.Video, Service.Voice};
           
            for (int i = 0; i < n; i++)
            {
                var call = Call.StartCall(HetNet._HetNet.MobileTerminals.PickRandom().MobileTerminalId, services.PickRandom());
                var callStartedEvent = new CallStartedEvent(DateTime.Now.AddMinutes(rnd.NextDouble() * 100), call);

                Log.Information($"---- Publishing event name: @{nameof(callStartedEvent)}; service: @{call.Service}; user: @{call.MobileTerminalId}");

                _mediator.Publish(callStartedEvent);
                //EventQueue.Enqueue(callStartedEvent);
                
                var callEndedEvent = new CallEndedEvent(call.CallId, call.MobileTerminalId, callStartedEvent.Time.AddMinutes(rnd.NextDouble() * 100));
                
                Log.Information($"---- Publishing event name: @{nameof(callEndedEvent)}; service: @{call.Service}; user: @{call.MobileTerminalId}");
                
               _mediator.Publish(callEndedEvent);

               //EventQueue.Enqueue(callEndedEvent);
            }

            Log.Information($"---- Serving Jobs In Queue");

            ServeQueue();
        }

        private void ServeQueue()
        {   
            var cac = JCallAdmissionControl.StartCAC();
            while(EventQueue.Any())
            {
                var @event = EventQueue.Dequeue();
                if(@event.GetType() == typeof(CallStartedEvent))
                {
                    var evt = (CallStartedEvent)@event;
                    cac.AdmitCall(evt.Call);
                }
                else 
                {
                    var evt = (CallEndedEvent)@event;
                    var mobileTerminal = HetNet._HetNet.MobileTerminals
                        .FirstOrDefault(x => x.MobileTerminalId == evt.MobileTerminalId);
                
                    mobileTerminal.TerminateCall(evt);
                }
            }
        }
    }
}