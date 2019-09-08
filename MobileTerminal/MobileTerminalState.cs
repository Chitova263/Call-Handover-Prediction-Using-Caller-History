using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VerticalHandoverPrediction.Mobile
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MobileTerminalState
    {
        Idle = 0 ,
        Voice = 1,
        Video = 2,
        Data = 3,
        VoiceVideo = 4,
        VoiceData = 5,
        VideoData = 6,
        VoiceDataVideo = 7,
    }
}