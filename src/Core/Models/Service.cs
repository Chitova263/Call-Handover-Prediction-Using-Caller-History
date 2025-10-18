namespace VerticalHandoverPrediction.Models
{
    [Flags]
    public enum Service
    {
        Voice = 0,
        Data = 1 << 0,
        Video = 1 << 1,
    }
}