using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction.Domain;

public class MobileTerminal
{
    public Guid MobileTerminalId { get; }
    public MobileTerminalState MobileTerminalState { get; private set; }
    public Dictionary<Guid, Call> ActiveCalls { get; set; }

    private MobileTerminal(Guid mobileTerminalId)
    {
        MobileTerminalId = mobileTerminalId;
        MobileTerminalState = MobileTerminalState.Idle;
    }

    public static MobileTerminal Create(Guid mobileTerminalId)
    {
        return new MobileTerminal(mobileTerminalId);
    }

    public void StartCall(Call call)
    {
        ActiveCalls.Add(call.CallId, call);
    }
        
    public void EndCall(Call call)
    {
        ActiveCalls.Remove(call.CallId);
    }
        
    private void AddState(MobileTerminalState state)
    {
        MobileTerminalState |= state;
    }
    
    private void RemoveState(MobileTerminalState state)
    {
        MobileTerminalState &= ~state;
    }
}