using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Mobile;

namespace Electron
{
    public class SimulationParameters
    {
        public string Calls { get; set; }
    }

    public class PredictionParameters
    {
        public Guid MobileTerminalId { get; set; }
        public Service Service { get; set; }
    }

    public class PredictionResults
    {
        public int Frequency { get; set; }
        public MobileTerminalState NextState { get; set; }
        public Dictionary<MobileTerminalState, int> FrequencyDictionary { get; set; }
    }
}