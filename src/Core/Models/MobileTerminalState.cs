using System;

namespace VerticalHandoverPrediction
{
    [Flags]
    public enum MobileTerminalState
    {
        Idle = 0,
        Voice = 1 << 0,
        Video = 1 << 1,
        Data = 1 << 2,
        VoiceVideo = Voice | Video,
        VideoData = Video | Data,
        VoiceData = Voice | Data,
        VoiceDataVideo = Voice | Video | Data,
    }
}
