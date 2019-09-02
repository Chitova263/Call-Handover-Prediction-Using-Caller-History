using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{
    public interface IHetNet
    {
        Guid HetNetId { get; }
        IList<Rat> Rats { get; }
        IList<MobileTerminal> MobileTerminals { get; }
        int VerticalHandovers { get; set; }
        int BlockedCalls { get; set; }
    }
}

