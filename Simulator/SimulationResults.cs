namespace VerticalHandoverPrediction.Simulator
{
    using CsvHelper.Configuration;
    public class SimulationResults
    {
        public int Handovers { get; set; }
        public int BlockedCalls { get; set; }
        public int FailedPredictions { get; set; }
        public int SuccessfulPredictions { get; set; }
        public int Calls { get; set; }
        public int TotalSessions { get; set; }
    }

    public class SimulationResultsMap: ClassMap<SimulationResults>
    {
        public SimulationResultsMap() => AutoMap();
    }
}