using System;
using System.Collections.Generic;
using System.Linq;
using Medallion.Collections;
using Serilog;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Commands;
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
            HetNet._HetNet.RandomCallsGenerated += n;
            
            foreach (var mt in HetNet._HetNet.MobileTerminals)
            {
                mt.Activated = false;
            }

            Random rnd = new Random();
           
            var services = new List<Service>{Service.Data, Service.Video, Service.Voice};
           
            for (int i = 0; i < n; i++)
            {
                var call = Call.StartCall(HetNet._HetNet.MobileTerminals.PickRandom().MobileTerminalId, services.PickRandom());
                
                var callStartedEvent = new CallStartedEvent(
                    DateTime.Now.AddMinutes(rnd.NextDouble() * 100),
                    call
                );

                EventQueue.Enqueue(callStartedEvent);
                
                var callEndedEvent = new CallEndedEvent(
                    call.CallId,
                    call.MobileTerminalId,
                    callStartedEvent.Time.AddMinutes(rnd.NextDouble() * 100)
                );

                EventQueue.Enqueue(callEndedEvent);
            }

            ServeQueue();
        }

        private void ServeQueue()
        {   
            var cac = new Cac.Cac();
            while(EventQueue.Any())
            {
                var @event = EventQueue.Dequeue();
                if(@event.GetType() == typeof(CallStartedEvent))
                {
                    var evt = (CallStartedEvent)@event;
                    cac.AdmitCall(evt);
                }
                else 
                {
                    var evt = (CallEndedEvent)@event;

                    var mobileTerminal = HetNet._HetNet.MobileTerminals
                        .FirstOrDefault(x => x.MobileTerminalId == evt.MobileTerminalId);
                    
                    //Find if call was considered
                    var call = HetNet._HetNet.Rats
                        .SelectMany(x => x.OngoingSessions)
                        .SelectMany(x => x.ActiveCalls)
                        .FirstOrDefault(x => x.CallId == evt.CallId);
                    
                    //if no session contains this call then it was never considered
                    if(call is null) 
                    {
                        HetNet._HetNet.CallEndedEventsRejected++;
                        Log.Warning($"CallStartedEventCorresponding o this {nameof(CallEndedEvent)} was rejected");
                    } 
                    else
                    {
                        mobileTerminal.EndCall(evt);
                    }
                }
            }
        }
    }
}