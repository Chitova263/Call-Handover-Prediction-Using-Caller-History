using System;
using System.Collections.Generic;
using System.Linq;
using Medallion.Collections;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VerticalHandoverPrediction.CallSession;
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
            var _mediator = DIContainer._Container.Container.GetRequiredService<IMediator>();
            var services = new List<Service>{Service.Data, Service.Video, Service.Voice};
            //Run Simulation
            for (int i = 0; i < n; i++)
            {
                var call = Call.StartCall(HetNet._HetNet.MobileTerminals.PickRandom().MobileTerminalId, services.PickRandom());
                var callStartedEvent = new CallStartedEvent(DateTime.Now, call);

                Log.Information($"---- Publishing event name: @{nameof(callStartedEvent)}");

                _mediator.Publish(callStartedEvent).Wait();
                
                var callEndedEvent = new CallEndedEvent(call.CallId, call.MobileTerminalId, DateTime.Now.AddMinutes(10));
                
                Log.Information($"---- Publishing event name: @{nameof(callEndedEvent)}");
                
                _mediator.Publish(callEndedEvent).Wait();
            }

            while (EventQueue.Any())
            {
                System.Console.WriteLine(EventQueue.Dequeue().Time);
            }

        }
    }
}