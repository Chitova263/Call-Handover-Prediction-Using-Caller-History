using VerticalHandoverPrediction.Models;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Simulator.Events;

namespace VerticalHandoverPrediction.CallAdmissionControlAlgorithms
{
    public interface IAlgorithm
    {
        void Admit(IEvent @event, Network network, BasicBandwidthUnits basicBandwidthUnits, HashSet<Guid> IgnoreEvents);
    }
}
