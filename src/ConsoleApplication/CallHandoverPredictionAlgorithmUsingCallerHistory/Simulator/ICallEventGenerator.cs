using Simulator.Event;
using Simulator.MultiModeMobileTerminal;

namespace Simulator;

public interface ICallEventGenerator
{
    EventPair GenerateStartAndEndCallEventPair(MobileTerminal terminal, DateTime startTime);
}