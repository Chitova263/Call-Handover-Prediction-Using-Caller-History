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
    }

    public class SimulationResultsMap: ClassMap<SimulationResults>
    {
        public SimulationResultsMap() => AutoMap();
    }
}