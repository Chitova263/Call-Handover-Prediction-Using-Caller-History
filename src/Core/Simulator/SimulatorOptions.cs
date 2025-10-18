namespace VerticalHandoverPrediction.Simulator
{
    public sealed class SimulatorOptions
    {
        public int NumberOfCalls { get; set; }
        public int VoiceBasicBandwidthUnits { get; set; }
        public int DataBasicBandwidthUnits { get; set; }
        public int VideoBasicBandwidthUnits { get; set; }
        public TimeSpan VoiceCallLifeTime { get; set; }
        public TimeSpan DataCallLifeSpan { get; set; }
        public TimeSpan VideoCallLifeSpan { get; set; }
        public bool PreserveCallLogs { get; set; }
        public bool UseInMemoryCallLogs { get; set; }
        public SimulatorOptions()
        {

        }
    }
}