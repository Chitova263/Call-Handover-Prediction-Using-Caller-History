﻿using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
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
