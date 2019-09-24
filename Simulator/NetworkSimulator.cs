namespace VerticalHandoverPrediction.Simulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Medallion.Collections;
    using VerticalHandoverPrediction.CallAdimissionControl;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Network;
    using VerticalHandoverPrediction.Simulator.Events;
    using VerticalHandoverPrediction.Simulator.Extensions;

    public class NetworkSimulator 
    {
        private static NetworkSimulator instance = null;
        private static Random random = new Random();
        private static List<Service> Services = new List<Service>{Service.Data, Service.Video, Service.Voice};
        public PriorityQueue<IEvent> EventQueue { get; }
        public bool SaveCallLogs { get; set; } = true;
        public IList<IEvent> Events { get; set; } //write to this list for successfull calls using the non predictive scheme

        private NetworkSimulator()
        {
            EventQueue = new PriorityQueue<IEvent>(new DateTimeComparer());
            Events = new List<IEvent>();
        }

        public static NetworkSimulator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkSimulator();
                }
                return instance;
            }
        }

        //First time you run this generate call history
        public void GenerateCalls(int numberOfCalls)
        {
            for (int i = 0; i < numberOfCalls; i++)
            {
                var service = Services.PickRandom();
                var mobileTerminal = HetNet.Instance.MobileTerminals.PickRandom();
                var call = new Call(mobileTerminal.MobileTerminalId, service);
                
                var callStartedEvent = new CallStartedEvent(
                    DateTime.Now.AddMinutes(1 + (120 - 1) * random.NextDouble()), 
                    call);
                
                var callEndedEvent = new CallEndedEvent(
                    call.CallId,
                    call.MobileTerminalId,
                    callStartedEvent.Time.AddMinutes(GenerateCallLifetime(service)));

                EventQueue.Enqueue(callStartedEvent);
                EventQueue.Enqueue(callEndedEvent);
            }
            ServeEventQueue(false); 
        }

        public void Run(bool predictive)
        {
            
            foreach (var evt in Events)
            {
                EventQueue.Enqueue(evt);
            }

            Events.Clear();
            HetNet.Instance.Reset();     
            ServeEventQueue(predictive);
        }

        private void ServeEventQueue(bool predictive)
        {   
            var cac = new Cac(predictive);
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

                    var mobileTerminal = HetNet.Instance.MobileTerminals
                        .FirstOrDefault(x => x.MobileTerminalId == evt.MobileTerminalId);
                    
                    //Find if call was considered
                    var call = HetNet.Instance.Rats
                        .SelectMany(x => x.OngoingSessions)
                        .SelectMany(x => x.ActiveCalls)
                        .FirstOrDefault(x => x.CallId == evt.CallId);
                    
                    //if no session contains this call then it was never considered
                    if(call is null) 
                    {
                        HetNet.Instance.CallEndedEventsRejected++;
                        //Log.Warning($"CallStartedEventCorresponding to {nameof(CallEndedEvent)} was rejected");
                    } 
                    else
                    {
                        Events.Add(evt);
                        mobileTerminal.EndCall(evt);
                    }
                }
            }
        }

        private double GenerateCallLifetime(Service service)
        {
            switch (service)
            {
                case Service.Voice:
                    return  1 + (10 - 1) * random.NextDouble();
                case Service.Data:
                    return  3 + (20 - 1) * random.NextDouble();
                case Service.Video:
                    return  5 + (60 - 1) * random.NextDouble();
                default:
                    return default(double);
            }
        }
    }
}