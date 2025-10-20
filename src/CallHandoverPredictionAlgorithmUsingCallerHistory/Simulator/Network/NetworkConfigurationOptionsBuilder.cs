using Simulator.MultiModeMobileTerminal;

namespace Simulator.Network;

public class NetworkConfigurationOptionsBuilder
{
    private readonly NetworkConfigurationOptions _networkConfigurationOptions = new();

    public NetworkConfigurationOptionsBuilder SetVoiceBandwidthRequirement(int basicBandwidthUnits)
    {
        _networkConfigurationOptions.BandwidthConfiguration.TryAdd(MobileTerminalEvent.StartVoiceCall,
            basicBandwidthUnits);
        return this;
    }

    public NetworkConfigurationOptionsBuilder SetDataBandwidthRequirement(int basicBandwidthUnits)
    {
        _networkConfigurationOptions.BandwidthConfiguration.TryAdd(MobileTerminalEvent.StartDataCall,
            basicBandwidthUnits);
        return this;
    }

    public NetworkConfigurationOptionsBuilder SetVideoBandwidthRequirement(int basicBandwidthUnits)
    {
        _networkConfigurationOptions.BandwidthConfiguration.TryAdd(MobileTerminalEvent.StartVideoCall,
            basicBandwidthUnits);
        return this;
    }

    public NetworkConfigurationOptionsBuilder SetCallArrivalRate(int callArrivalRate)
    {
        _networkConfigurationOptions.CallArrivalRate  = callArrivalRate;
        return this;
    }
    
    public NetworkConfigurationOptions Build() => _networkConfigurationOptions;
}