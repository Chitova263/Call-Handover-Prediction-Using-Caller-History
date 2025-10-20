namespace Simulator.MultiModeMobileTerminal;

/// <summary>
/// Represents the operational state of a multimode <see cref="MobileTerminal"/>.
/// </summary>
/// <remarks>
/// Supports combinations of states using bitwise flags.
/// </remarks>
/// <seealso cref="MobileTerminal"/>
[Flags]
public enum MobileTerminalState
{
    /// <summary>
    /// The <see cref="MobileTerminal"/> is idle and not engaged in any call.
    /// </summary>
    Idle  = 0,
    
    /// <summary>
    /// The <see cref="MobileTerminal"/> is engaged in a voice call.
    /// </summary>
    VoiceCallActive = 1 << 0,
    
    /// <summary>
    /// The <see cref="MobileTerminal"/> is transmitting or receiving data.
    /// </summary>
    DataCallActive  = 1 << 1,
    
    /// <summary>
    /// The <see cref="MobileTerminal"/> is streaming or transmitting video.
    /// </summary>
    VideoCallActive = 1 << 2,
}