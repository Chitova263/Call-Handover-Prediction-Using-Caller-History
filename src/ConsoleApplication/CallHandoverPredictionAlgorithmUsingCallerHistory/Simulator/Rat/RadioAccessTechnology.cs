using Simulator.MultiModeMobileTerminal;

namespace Simulator.Rat;

public class RadioAccessTechnology
{
    public Guid RadioAccessTechnologyId { get; }
    private readonly Dictionary<Guid, Session> _sessions;
    public IReadOnlyDictionary<Guid, Session> Sessions => _sessions.AsReadOnly();
    
    private readonly HashSet<MobileTerminalEvent> _supportedMobileTerminalEvents;
    public IReadOnlySet<MobileTerminalEvent> SupportedMobileTerminalEvents => _supportedMobileTerminalEvents;
    
    public int BasicBandwidthUnitsCapacity { get; private set; }

    private RadioAccessTechnology(HashSet<MobileTerminalEvent> supportedMobileTerminalEvents, int basicBandwidthUnitsCapacity)
    {
        BasicBandwidthUnitsCapacity = basicBandwidthUnitsCapacity;
        RadioAccessTechnologyId = Guid.NewGuid();
        _sessions = [];
        _supportedMobileTerminalEvents = supportedMobileTerminalEvents;
    }

    public static RadioAccessTechnology Create(
        HashSet<MobileTerminalEvent> supportedMobileTerminalEvents,
        int basicBandwidthUnitsCapacity
        )
    {
        if (basicBandwidthUnitsCapacity < 1)
        {
            throw new ArgumentException(nameof(basicBandwidthUnitsCapacity));
        }
        return new RadioAccessTechnology(supportedMobileTerminalEvents, basicBandwidthUnitsCapacity);
    }
}   
