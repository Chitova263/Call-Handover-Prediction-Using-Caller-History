namespace Simulator.Rat;

public sealed class RadioAccessTechnologyOptionsBuilder
{
    private readonly RadioAccessTechnologyOptions _options = new()
    {
        IsVoiceEnabled = false,
        IsDataEnabled = false,
        IsVideoEnabled = false,
        MaxBasicBandwidthUnits = 10
    };
    
    public RadioAccessTechnologyOptionsBuilder WithVoiceEnabled(bool isVoiceEnabled = true)
    {
        _options.IsVoiceEnabled = isVoiceEnabled;
        return this;
    }
    
    public RadioAccessTechnologyOptionsBuilder WithDataEnabled(bool isDataEnabled = true)
    {
        _options.IsDataEnabled = isDataEnabled;
        return this;
    }
    
    public RadioAccessTechnologyOptionsBuilder WithVideoEnabled(bool isVideoEnabled = true)
    {
        _options.IsVideoEnabled = isVideoEnabled;
        return this;
    }
    
    public RadioAccessTechnologyOptionsBuilder HasMaxBasicBandwidthUnits(int maxBasicBandwidthUnits)
    {
        _options.MaxBasicBandwidthUnits = maxBasicBandwidthUnits;
        return this;
    }
    
    public RadioAccessTechnologyOptions Build()
    {
        return _options;
    }
}