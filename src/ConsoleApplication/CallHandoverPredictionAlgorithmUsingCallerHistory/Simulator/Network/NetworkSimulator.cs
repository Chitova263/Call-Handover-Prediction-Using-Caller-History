using Simulator.CallAdmissionControlAlgorithm;
using Simulator.Event;
using Simulator.MultiModeMobileTerminal;

namespace Simulator.Network;

public class NetworkSimulator
{
    private readonly HeterogeneousNetwork _heterogeneousNetwork;
    private readonly ICallEventGenerator _callEventGenerator;
    private static readonly PriorityQueue<NetworkEvent, DateTime> _queue = new();

    private NetworkSimulator(HeterogeneousNetwork heterogeneousNetwork, ICallEventGenerator callEventGenerator)
    {
        _heterogeneousNetwork = heterogeneousNetwork;
        _callEventGenerator = callEventGenerator;
    }

    public static NetworkSimulator Create(HeterogeneousNetwork heterogeneousNetwork, ICallEventGenerator callEventGenerator)
    {
        return new NetworkSimulator(heterogeneousNetwork, callEventGenerator);
    }

    public void Run(ICallAdmissionControlAlgorithm callAdmissionControlAlgorithm)
    {
        // Generate events
        for (int i = 0; i < 100; i++)
        {
            // Effect of mobile terminal at different network congestions
            // Grab a random mobile terminal
            var eventPair = _callEventGenerator.GenerateStartAndEndCallEventPair(_heterogeneousNetwork.MobileTerminals.Values.First(), DateTime.Now);
            _queue.Enqueue(eventPair.StartEvent, eventPair.StartEvent.TimeStamp);
            _queue.Enqueue(eventPair.EndEvent, eventPair.EndEvent.TimeStamp);
        }
        // Get the mobile terminal
        // Generate the events poisson distribution
        while (_queue.Count > 0)
        {
            var eventPair = _queue.Dequeue();
            TryAdmitCall(callAdmissionControlAlgorithm, eventPair, out var session);
        }
    }

    private void TryAdmitCall(ICallAdmissionControlAlgorithm callAdmissionControlAlgorithm, NetworkEvent @event, out Session session)
    {
        _heterogeneousNetwork.TryAdmitCall(
            callAdmissionControlAlgorithm,
            DateTime.Now,
            MobileTerminalEvent.StartVoiceCall,
            _heterogeneousNetwork.MobileTerminals.Values.First(),
            out session
        );
    }
}