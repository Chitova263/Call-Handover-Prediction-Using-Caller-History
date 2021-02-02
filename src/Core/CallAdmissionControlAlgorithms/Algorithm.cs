using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalHandoverPrediction
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
