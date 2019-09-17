using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VerticalHandoverPrediction.CallSession
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Service
    {
        Voice,
        Data,
        Video,
    }
}