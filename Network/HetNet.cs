using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Network
{

    public sealed class HetNet : IHetNet
    {
        private static HetNet instance = null;
        private static readonly object padlock = new object();

        public Guid HetNetId { get; private set; }
        public IList<Rat> Rats { get; private set; }
        public IList<MobileTerminal> MobileTerminals { get; private set; }
        public int VerticalHandovers { get; set; }
        public int BlockedCalls { get; set; }
        public int FailedPredictions {get; set; }
        public int SuccessfulPredictions { get; set; }

        private HetNet()
        {
            HetNetId = Guid.NewGuid();
            //All the rats in the network
            Rats = new List<Rat>();
            //All the mobile terminals in the network
            MobileTerminals = new List<MobileTerminal>();
        }

        public static HetNet _HetNet
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new HetNet();
                    }
                    return instance;
                }
            }
        }
    }
}

