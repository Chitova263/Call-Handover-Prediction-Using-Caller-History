using Simulator.MultiModeMobileTerminal;

namespace Simulator;

public class Call
{
    public Guid CallId { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; private set; }
    public MobileTerminalEvent MobileTerminalEvent { get; }

    public Session Session { get; set; }
    
    private Call(DateTime startTime, MobileTerminalEvent mobileTerminalEvent)
    {
        CallId = Guid.NewGuid();
        StartTime = startTime;
        MobileTerminalEvent = mobileTerminalEvent;
    }

    public static Call StartVoiceCall(DateTime startTime)
    {
        return new Call(startTime, MobileTerminalEvent.StartVoiceCall);
    }
    
    public static Call StartDataCall(DateTime startTime)
    {
        return new Call(startTime, MobileTerminalEvent.StartDataCall);
    }
    
    public static Call StartVideoCall(DateTime startTime)
    {
        return new Call(startTime, MobileTerminalEvent.StartVideoCall);
    }

    public void TerminateCall(Guid callId, DateTime endTime)
    {
        if (StartTime >= endTime)
            throw new Exception("EndTime must be less than StartTime");
        EndTime = endTime;
    }
}