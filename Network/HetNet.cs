using System;
using System.Collections.Generic;

namespace VerticalHandoverPrediction
{

    //HetNet contains a pool of RATs
    public class HetNet : IHetNet
    {
        public Guid HetNetId { get; private set; }
        public IList<RAT> RATs { get; private set; } = new List<RAT>();
        public int NumberOfHandovers { get; private set; }

        private HetNet(IList<RAT> rats)
        {
            HetNetId = Guid.NewGuid();
            RATs = rats;
        }

        public static HetNet InitializeHetNet(IList<RAT> rats)
        {
            var hetnet = new HetNet(rats);
            return hetnet;
        }
    }
}