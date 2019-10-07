namespace VerticalHandoverPrediction.CallSession
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum Service
    {
        Voice,
        Data,
        Video,
    }
}