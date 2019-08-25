namespace Handoverprediction.VerticalHandover
{
    public class RAT
    {
        public Service Service { get; set; }
        public int Capacity { get; set; }
        public int BasebandUnits { get; set; }
    }

    public enum Service
    {
        Voice = 1,
        Data = 2,
        VoiceAndData = 3,
        VoiceVideoData = 4,
    }
}