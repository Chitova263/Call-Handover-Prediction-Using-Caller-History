namespace VerticalHandoverPrediction
{
    public sealed class SimulatorOptions
    {
        public int NumberOfCalls { get; set; }
        
        private SimulatorOptions()
        {

        }

        public static SimulatorOptions CreateDefaultSimulatorOptions()
        {
            return new SimulatorOptions();
        }
    }
}