using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VerticalHandoverPrediction.CallSession
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Service
    {
        Voice = 1,
        Data = 2,
        Video = 3,
    }
}