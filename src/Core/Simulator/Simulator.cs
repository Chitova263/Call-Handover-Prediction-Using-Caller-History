using Medallion.Collections;
using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction
{
    public sealed class Simulator
    {
        private readonly PriorityQueue<IEvent> _eventQueue;
        private readonly SimulatorOptions _simulatorOptions;
        private readonly Network _network;
        private static readonly List<Service> _services = new List<Service> { Service.Voice, Service.Data, Service.Video };
        
        // Events that materialize to calls
        private Dictionary<Guid, IEvent> SuccessfulEvents { get; set; }

        public Simulator(Network network, SimulatorOptions simulatorOptions)
        {
            _eventQueue = new PriorityQueue<IEvent>(new DateTimeComparer());
            _network = network;
            _simulatorOptions = simulatorOptions;
        }

        public Result Run<TAlgorithm>() where TAlgorithm : Algorithm
        {
            GenerateEvents();
            foreach (var @event in _eventQueue)
                HandleEvent<TAlgorithm>(@event);
            
            return new Result();
        }

        private void GenerateEvents()
        {
            for (int i = 0; i < _simulatorOptions.NumberOfCalls; i++)
            {
                var service = _services.PickRandom();
                var mobileTerminal = _network.MobileTerminals.PickRandom().Value;

                var startEvent = new CallStartedEvent
                {
                    MobileTerminal = mobileTerminal,
                    Service = service,
                };

                var endEvent = new CallEndedEvent
                {
                    MobileTerminal = mobileTerminal,
                    CallStartedEventId = startEvent.EventId,
                    Timestamp = startEvent.Timestamp.AddMinutes(20),
                };

                _eventQueue.Enqueue(startEvent);
                _eventQueue.Enqueue(endEvent);
            }
        }

        private void HandleEvent<TAlgorithm>(IEvent @event) where TAlgorithm : Algorithm
        {
            Algorithm algorithm = null;
            if (typeof(TAlgorithm) == typeof(NonPredictiveAlgorithm))
            {
                algorithm = new NonPredictiveAlgorithm();
            } 
            else
            {
                algorithm = new PredictiveAlgorithm();
            }


            if (@event is CallStartedEvent evt)
            {
                var mobileTerminal = evt.MobileTerminal;
                //1. if there is no call
                if(!mobileTerminal.IsActive)
                    algorithm.Admit(@event);
            }
            else if ( @event is CallEndedEvent)
            {

            }
        }
    }
}
