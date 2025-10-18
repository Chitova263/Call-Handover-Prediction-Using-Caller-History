using Medallion.Collections;
using VerticalHandoverPrediction.CallAdmissionControlAlgorithms;
using VerticalHandoverPrediction.Extensions;
using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.Simulator
{
    public sealed class Simulator
    {
        private readonly PriorityQueue<IEvent> _eventQueue;
        private readonly SimulatorOptions _simulatorOptions;
        private readonly Network _network;
        private readonly List<Service> _services = new List<Service> { Service.Voice, Service.Data, Service.Video };
        private readonly HashSet<Guid> _ignoreEvents;

        public Simulator(Network network, SimulatorOptions simulatorOptions)
        {
            _eventQueue = new PriorityQueue<IEvent>(new DateTimeComparer());
            _network = network;
            _simulatorOptions = simulatorOptions;
            _ignoreEvents = new HashSet<Guid>();
        }

        public Result Run<TAlgorithm>() 
            where TAlgorithm : Algorithm, new()
        {
            var algorithm = new TAlgorithm();
            GenerateEvents();
            foreach (var @event in _eventQueue)
            {
                if (@event is CallEndedEvent evt)
                {
                    if (_ignoreEvents.Contains(evt.CallStartedEventId))
                        continue; //DO NOT DISPATCH EVENT
                }
                else
                {
                    algorithm.Admit(
                        @event as CallStartedEvent,
                        _network,
                        new BasicBandwidthUnits(_simulatorOptions.VoiceBasicBandwidthUnits, _simulatorOptions.DataBasicBandwidthUnits, _simulatorOptions.VideoBasicBandwidthUnits),
                        _ignoreEvents);
                }
            }

            return algorithm._result;
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
    }
}
