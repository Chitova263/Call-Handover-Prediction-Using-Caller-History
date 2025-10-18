using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.CallAdmissionControlAlgorithms
{
    public abstract class Algorithm : IAlgorithm
    {
        public readonly Result _result;

        public Algorithm()
        {
            _result = new Result();
        }

        public abstract void Admit(
            IEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents);
    }
}
