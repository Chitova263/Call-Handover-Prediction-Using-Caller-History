using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.CallAdmissionControlAlgorithms
{
    public sealed class PredictiveAlgorithm : Algorithm
    {
        public override void Admit(
            IEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents)
        {
            throw new NotImplementedException();
        }
    }
}
