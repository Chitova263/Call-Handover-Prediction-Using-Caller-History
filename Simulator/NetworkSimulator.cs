namespace VerticalHandoverPrediction.Simulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.Distributions;
    using Medallion.Collections;
    using VerticalHandoverPrediction.CallSession;
    using VerticalHandoverPrediction.Network;
    using VerticalHandoverPrediction.Simulator.Events;
    using VerticalHandoverPrediction.Simulator.Extensions;

    public class NetworkSimulator 
    {
        private static NetworkSimulator instance = null;
        private static readonly object padlock = new object();
        public PriorityQueue<IEvent> EventQueue { get; }
        public bool UseCallLogs { get; set; } = true;
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

        public void Run(int n, bool test, bool predictive)
        {
            if(test){
                
                foreach (var mobileTerminal in HetNet.Instance.MobileTerminals)
                {
                    mobileTerminal.SetActive(false);
                }

                var poisson = new Poisson(12); 
                var exponential = new Exponential(0.01);

                var services = new List<Service>{Service.Data, Service.Video, Service.Voice};
            
                for (int i = 0; i < n; i++)
                {
                    var call = new Call(HetNet.Instance.MobileTerminals.PickRandom().MobileTerminalId, services.PickRandom());
                    
                    var callStartedEvent = new CallStartedEvent(
                        DateTime.Now.AddMinutes(poisson.Sample()*120), //0.1 calls per unit time
                        call
                    );

                    EventQueue.Enqueue(callStartedEvent);
                    
                    var callEndedEvent = new CallEndedEvent(
                        call.CallId,
                        call.MobileTerminalId,
                        callStartedEvent.Time.AddMinutes(exponential.Sample())
                    );

                    EventQueue.Enqueue(callEndedEvent);
                }
            }
            else
            {
                var startEvents = Utils.CsvUtils._Instance.Read<StartEventMap ,StartEvent>($"{Environment.CurrentDirectory}/start.csv");
                var endEvents = Utils.CsvUtils._Instance.Read<EndEventMap ,EndEvent>($"{Environment.CurrentDirectory}/end.csv");
                foreach (var evt in startEvents)
                {
                    EventQueue.Enqueue(new CallStartedEvent(evt.Time, new Call(evt.MobileTerminalId, evt.Service, evt.CallId)));
                }
                foreach (var evt in endEvents)
                {
                   EventQueue.Enqueue(new CallEndedEvent(evt.CallId, evt.MobileTerminalId, evt.Time));
                }
                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
                HetNet.Instance.Reset();
            }
            
            ServeQueue(predictive);
        }

        private void ServeQueue(bool predictive)
        {   
            var cac = new Cac.Cac(predictive);
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
                        //Log.Warning($"CallStartedEventCorresponding o this {nameof(CallEndedEvent)} was rejected");
                    } 
                    else
                    {
                        Utils.CsvUtils._Instance.Write<CallEndedEventMap, CallEndedEvent>(evt, $"{Environment.CurrentDirectory}/end.csv");
                        mobileTerminal.EndCall(evt);
                    }
                }
            }
        }
    }
}