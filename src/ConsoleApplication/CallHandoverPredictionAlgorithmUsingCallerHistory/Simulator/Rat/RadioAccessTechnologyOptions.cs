namespace Simulator.Rat;

public class RadioAccessTechnologyOptions
{
    public bool IsVoiceEnabled { get; set; } = true;
    public bool IsDataEnabled { get; set; }
    public bool IsVideoEnabled { get; set; }
    public int MaxBasicBandwidthUnits { get; set; } = 10;
}