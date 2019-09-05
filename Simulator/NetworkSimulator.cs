using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;

namespace VerticalHandoverPrediction.Simulator
{
    public sealed class NetworkSimulator 
    {
        private static NetworkSimulator instance = null;
        private static readonly object padlock = new object();

        private NetworkSimulator()
        {

        }

        public static NetworkSimulator _NetworkSimulator
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NetworkSimulator();
                    }
                    return instance;
                }
            }
        }

        public void Run(int n)
        {
            var services = new List<Service>{Service.Data, Service.Video, Service.Voice};
            //Run Simulation
            for (int i = 0; i < n; i++)
            {
                Call.StartCall(HetNet._HetNet.MobileTerminals.PickRandom().MobileTerminalId, services.PickRandom());
            }
        }
    }
}