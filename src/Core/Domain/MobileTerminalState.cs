using System;

namespace VerticalHandoverPrediction.Domain;

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
    Voice = 1 << 0,
    
    /// <summary>
    /// The <see cref="MobileTerminal"/> is transmitting or receiving data.
    /// </summary>
    Data  = 1 << 1,
    
    /// <summary>
    /// The <see cref="MobileTerminal"/> is streaming or transmitting video.
    /// </summary>
    Video = 1 << 2,
}