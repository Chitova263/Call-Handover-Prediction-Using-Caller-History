using System;

namespace VerticalHandoverPrediction
{
    public sealed class SimulatorBuilder
    {
        private readonly Network _network;
        private readonly SimulatorOptions _simulatorOptions;

        public SimulatorBuilder(Network network, Action<SimulatorOptions> action)
        {
            _network = network;
            var options = SimulatorOptions.CreateDefaultSimulatorOptions();
            action(options);
            _simulatorOptions = options;
        }
        
        public Simulator Build()
        {
            return new Simulator(_network, _simulatorOptions); 
        }
    }
}