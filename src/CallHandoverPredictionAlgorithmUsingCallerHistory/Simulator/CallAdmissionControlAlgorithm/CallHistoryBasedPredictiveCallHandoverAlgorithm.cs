using Simulator.MultiModeMobileTerminal;
using Simulator.Network;
using Simulator.Rat;

namespace Simulator.CallAdmissionControlAlgorithm;

public class CallHistoryBasedPredictiveCallHandoverAlgorithm : ICallAdmissionControlAlgorithm
{
    public bool TryAdmitCall(DateTime startTime, MobileTerminalEvent mobileTerminalEvent, MobileTerminal mobileTerminal,
        Dictionary<Guid, RadioAccessTechnology> radioAccessTechnologies,
        NetworkConfigurationOptions networkConfigurationOptions, out Session session)
    {
        throw new NotImplementedException();
    }
}