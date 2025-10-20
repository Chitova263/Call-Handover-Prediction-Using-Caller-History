using FiniteStateMachine;

namespace Simulator.MultiModeMobileTerminal;

public class MobileTerminal
{
    private readonly FiniteStateMachine<MobileTerminalState, MobileTerminalEvent> _mobileTerminalStateMachine;
    public Guid MobileTerminalId { get; }
    private readonly List<Session> _sessions;
    public IReadOnlyList<Session> Sessions => _sessions.AsReadOnly();

    private MobileTerminal(FiniteStateMachine<MobileTerminalState, MobileTerminalEvent>  mobileTerminalStateMachine)
    {
        _mobileTerminalStateMachine = mobileTerminalStateMachine;
        MobileTerminalId = Guid.NewGuid();
        _sessions = [];
    }

    public static MobileTerminal Create(
        FiniteStateMachine<MobileTerminalState, MobileTerminalEvent> mobileTerminalStateMachine
        )
    {
        return new MobileTerminal(mobileTerminalStateMachine);
    }
    
    public bool IsValidStateTransition(MobileTerminalEvent mobileTerminalEvent)
    {
        return _mobileTerminalStateMachine.IsValidStateTransition(mobileTerminalEvent);
    }
 
    private Session? GetCurrentSession()
    {
        var session = _sessions.LastOrDefault();
        if (session == null)
        {
            return null;
        }
        return session;
    }
    
    private bool IsCallAdmissible(DateTime startTime, MobileTerminalEvent startVoiceCall)
    {
        return true;
    }

}