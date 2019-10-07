namespace VerticalHandoverPrediction.Simulator
{
    using CsvHelper.Configuration;
    public class SimulationResults
    {
        public int Calls { get; set; }
        public int PredictiveHandovers { get; set; }
        public int NonPredictiveHandovers { get; set; }
        public int PredictiveBlockedCalls { get; set; }
        public int NonPredictiveBlockedCalls { get; set; }
        public int TotalSessions { get; set; }
        public int FailedPredictions { get; set; }
        public int SuccessfulPredictions { get; set; }
        public double DataAvoided { get; set; }
        public double VoiceAvoided { get; set; }
        public double VideoAvoided { get; set; }
        public double TotalAvoided { get; set; }
        public int VoiceCalls { get; set; }
        public int VideoCalls { get; set; }
        public int DataCalls { get; set; }
        public int NonPredictiveVoiceHandovers { get; set; }
        public int NonPredictiveDataHandovers { get; set; }
        public int NonPredictiveVideoHandovers { get; set; }
        public int PredictiveVoiceHandovers { get; set; }
        public int PredictiveDataHandovers { get; set; }
        public int PredictiveVideoHandovers { get; set; }
    }

    public class SimulationResultsMap: ClassMap<SimulationResults>
    {
        public SimulationResultsMap() => AutoMap();
    }
}