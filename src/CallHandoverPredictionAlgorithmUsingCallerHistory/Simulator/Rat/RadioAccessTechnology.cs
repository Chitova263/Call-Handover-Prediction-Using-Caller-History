using Simulator.MultiModeMobileTerminal;

namespace Simulator.Rat
{
    /// <summary>
    /// Represents a radio access technology within the simulator, managing sessions and supported mobile terminal events.
    /// </summary>
    public class RadioAccessTechnology
    {
        /// <summary>
        /// Gets the unique identifier for the radio access technology instance.
        /// </summary>
        public Guid RadioAccessTechnologyId { get; }

        private readonly Dictionary<Guid, Session> _sessions = new();
        
        /// <summary>
        /// Gets a read-only view of the active sessions associated with this radio access technology.
        /// </summary>
        public IReadOnlyDictionary<Guid, Session> Sessions => _sessions;

        private readonly HashSet<MobileTerminalEvent> _supportedMobileTerminalEvents;
        
        /// <summary>
        /// Gets a read-only view of the mobile terminal events supported by this radio access technology.
        /// </summary>
        public IReadOnlySet<MobileTerminalEvent> SupportedMobileTerminalEvents => _supportedMobileTerminalEvents;

        /// <summary>
        /// Gets the capacity in basic bandwidth units for this radio access technology.
        /// </summary>
        public int BasicBandwidthUnitsCapacity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioAccessTechnology"/> class.
        /// </summary>
        /// <param name="supportedMobileTerminalEvents">The set of supported mobile terminal events.</param>
        /// <param name="basicBandwidthUnitsCapacity">The basic bandwidth units capacity.</param>
        /// <exception cref="ArgumentNullException">Thrown when supportedMobileTerminalEvents is null.</exception>
        /// <exception cref="ArgumentException">Thrown when basicBandwidthUnitsCapacity is less than 1.</exception>
        private RadioAccessTechnology(HashSet<MobileTerminalEvent> supportedMobileTerminalEvents, int basicBandwidthUnitsCapacity)
        {
            if (basicBandwidthUnitsCapacity < 1)
                throw new ArgumentException("Basic bandwidth units capacity must be greater than zero.", nameof(basicBandwidthUnitsCapacity));
            
            RadioAccessTechnologyId = Guid.NewGuid();
            BasicBandwidthUnitsCapacity = basicBandwidthUnitsCapacity;
            _supportedMobileTerminalEvents = supportedMobileTerminalEvents ?? throw new ArgumentNullException(nameof(supportedMobileTerminalEvents), "Supported mobile terminal events cannot be null.");
        }

        /// <summary>
        /// Creates a new instance of <see cref="RadioAccessTechnology"/>.
        /// </summary>
        /// <param name="supportedMobileTerminalEvents">The set of supported mobile terminal events.</param>
        /// <param name="basicBandwidthUnitsCapacity">The basic bandwidth units capacity.</param>
        /// <returns>A new <see cref="RadioAccessTechnology"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supportedMobileTerminalEvents is null.</exception>
        /// <exception cref="ArgumentException">Thrown when basicBandwidthUnitsCapacity is less than 1.</exception>
        public static RadioAccessTechnology Create(HashSet<MobileTerminalEvent> supportedMobileTerminalEvents, int basicBandwidthUnitsCapacity)
        {
            if (supportedMobileTerminalEvents == null)
                throw new ArgumentNullException(nameof(supportedMobileTerminalEvents), "Supported mobile terminal events cannot be null.");

            if (basicBandwidthUnitsCapacity < 1)
                throw new ArgumentException("Basic bandwidth units capacity must be greater than zero.", nameof(basicBandwidthUnitsCapacity));

            return new RadioAccessTechnology(supportedMobileTerminalEvents, basicBandwidthUnitsCapacity);
        }
    }
}
