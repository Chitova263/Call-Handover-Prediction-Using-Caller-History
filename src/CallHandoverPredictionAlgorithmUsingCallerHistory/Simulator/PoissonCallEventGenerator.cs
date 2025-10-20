using Simulator.Event;
using Simulator.MultiModeMobileTerminal;

namespace Simulator;

public class PoissonCallEventGenerator : ICallEventGenerator
{
    private readonly double _meanCallArrivalRatePerSecond; // mean arrival rate (events per second)
    private readonly Random _random = new();
    private readonly double _meanCallDurationRate;      // mean call duration rate (1 / mean duration seconds)

    private static readonly Dictionary<MobileTerminalEvent, MobileTerminalEvent> MobileTerminalEvents = new()
    {
        [MobileTerminalEvent.StartVoiceCall] = MobileTerminalEvent.StopVoiceCall,
        [MobileTerminalEvent.StartDataCall] = MobileTerminalEvent.StopDataCall,
        [MobileTerminalEvent.StartVideoCall] = MobileTerminalEvent.StopVideoCall,
    };

    public PoissonCallEventGenerator(double meanArrivalRatePerSecond, double meanDurationSeconds)
    {
        if (meanArrivalRatePerSecond <= 0)
            throw new ArgumentException("Mean arrival rate must be positive.", nameof(meanArrivalRatePerSecond));
        if (meanDurationSeconds <= 0)
            throw new ArgumentException("Mean duration must be positive.", nameof(meanDurationSeconds));

        _meanCallArrivalRatePerSecond = meanArrivalRatePerSecond;
        _meanCallDurationRate = 1.0 / meanDurationSeconds;
    }

    // Exponentially distributed random variable (seconds)
    private double GetNextExponentiallyDistributedRandomVariableInSeconds(double rate) => -Math.Log(1.0 - _random.NextDouble()) / rate;
    
    // Generate sequence of call start and end events
    public EventPair GenerateStartAndEndCallEventPair(MobileTerminal terminal, DateTime startTime) 
    {
        var currentTime = startTime;
        // Determine when the next call starts
        var interArrival = GetNextExponentiallyDistributedRandomVariableInSeconds(_meanCallArrivalRatePerSecond);
        currentTime = currentTime.AddSeconds(interArrival);

        var callStartTime = currentTime;
        var callDuration = GetNextExponentiallyDistributedRandomVariableInSeconds(_meanCallDurationRate);
        var callEndTime = callStartTime.AddSeconds(callDuration);
        var correlationId = Guid.NewGuid();

        var startMobileTerminalEvent = MobileTerminalEvents.Keys.ElementAt(_random.Next(MobileTerminalEvents.Count));
            
        NetworkEvent startEvent = new()
        {
            MobileTerminal = terminal,
            MobileTerminalEvent = startMobileTerminalEvent,
            EventCorrelationId = correlationId,
            TimeStamp = callStartTime
        };

        NetworkEvent endEvent = new()
        {
            MobileTerminal = terminal,
            MobileTerminalEvent = MobileTerminalEvents[startMobileTerminalEvent],
            EventCorrelationId = correlationId,
            TimeStamp = callEndTime
        };
        return new EventPair(startEvent, endEvent);
    }
}