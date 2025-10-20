using Simulator.CallAdmissionControlAlgorithm;
using Simulator.MultiModeMobileTerminal;
using Simulator.Rat;

namespace Simulator.Network;

public class HeterogeneousNetwork
{
    private readonly Dictionary<Guid, MobileTerminal> _mobileTerminals;
    public IReadOnlyDictionary<Guid, MobileTerminal> MobileTerminals => _mobileTerminals.AsReadOnly();
    
    private readonly Dictionary<Guid, RadioAccessTechnology> _radioAccessTechnologies;
    public IReadOnlyDictionary<Guid, RadioAccessTechnology> RadioAccessTechnologies => _radioAccessTechnologies.AsReadOnly();
    private readonly NetworkConfigurationOptions _networkConfigurationOptions;
    
    public bool TryAdmitCall(
        ICallAdmissionControlAlgorithm callAdmissionControlAlgorithm,
        DateTime startTime, 
        MobileTerminalEvent mobileTerminalEvent, 
        MobileTerminal mobileTerminal,
        out Session session)
    {
        return callAdmissionControlAlgorithm.TryAdmitCall(
            startTime, 
            mobileTerminalEvent, 
            mobileTerminal,
            _radioAccessTechnologies, 
            _networkConfigurationOptions,
            out  session);
    }

    public bool IsMobileTerminalRegistered(MobileTerminal  mobileTerminal)
    {
        return _mobileTerminals.ContainsKey(mobileTerminal.MobileTerminalId);
    }
    
    private HeterogeneousNetwork(
        Dictionary<Guid, MobileTerminal> mobileTerminals,
        Dictionary<Guid, RadioAccessTechnology> radioAccessTechnologies,
        NetworkConfigurationOptions networkConfigurationOptions)
    {
        _mobileTerminals = mobileTerminals;
        _radioAccessTechnologies = radioAccessTechnologies;
        _networkConfigurationOptions = networkConfigurationOptions;
    }
    
    public static HeterogeneousNetwork Create(
        Dictionary<Guid, MobileTerminal> mobileTerminals,
        Dictionary<Guid, RadioAccessTechnology> radioAccessTechnologies,
        NetworkConfigurationOptions networkConfigurationOptions)
    {
        return new HeterogeneousNetwork(mobileTerminals, radioAccessTechnologies, networkConfigurationOptions);
    }
}