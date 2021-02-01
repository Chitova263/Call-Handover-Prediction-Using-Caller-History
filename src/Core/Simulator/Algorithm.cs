﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalHandoverPrediction
{
    public abstract class Algorithm
    {
        public abstract void Admit(
            IEvent @event,
            Network network,
            BasicBandwidthUnits basicBandwidthUnits,
            HashSet<Guid> IgnoreEvents);
    }
}
