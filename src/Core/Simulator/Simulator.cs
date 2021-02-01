using Medallion.Collections;
using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{
    public sealed class Simulator
    {
        private readonly PriorityQueue<IEvent> _eventQueue;
        private readonly SimulatorOptions _simulatorOptions;
        private readonly Network _network;
        private static readonly List<Service> _services = new List<Service> { Service.Voice, Service.Data, Service.Video };
       
        public HashSet<Guid> IgnoreEvents { get; }

        // Events that materialize to calls
        private Dictionary<Guid, IEvent> SuccessfulEvents { get; set; }

        public Simulator(Network network, SimulatorOptions simulatorOptions)
        {
            _eventQueue = new PriorityQueue<IEvent>(new DateTimeComparer());
            _network = network;
            _simulatorOptions = simulatorOptions;
            IgnoreEvents = new HashSet<Guid>();
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
               
            }
            else if (@event is CallEndedEvent)
            {

            }
        }
    }
}
