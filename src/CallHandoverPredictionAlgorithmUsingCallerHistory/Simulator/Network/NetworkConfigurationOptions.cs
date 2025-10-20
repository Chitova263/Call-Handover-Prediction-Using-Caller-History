using Simulator.MultiModeMobileTerminal;

namespace Simulator.Network;

public sealed class NetworkConfigurationOptions
{
    public IDictionary<MobileTerminalEvent, int> BandwidthConfiguration { get; set; } = new Dictionary<MobileTerminalEvent, int>
    {
        [MobileTerminalEvent.StartVoiceCall] = 1,
        [MobileTerminalEvent.StartDataCall] = 2,
        [MobileTerminalEvent.StartVideoCall] = 3,
    };
    public int CallArrivalRate { get; set; }
}