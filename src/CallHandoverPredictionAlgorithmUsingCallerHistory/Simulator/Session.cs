using Simulator.Rat;

namespace Simulator;

public class Session
{
    public Guid SessionId { get; set; }
    private List<Call> CallHistory { get; }
    public RadioAccessTechnology RadioAccessTechnology { get; set; }
}