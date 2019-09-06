using System.Collections;
using System.Collections.Generic;
using Medallion.Collections;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;

namespace VerticalHandoverPrediction.Simulator
{
    public class EventQueueComparer : IComparer<CallStartedEvent>
    {
        public int Compare(CallStartedEvent x, CallStartedEvent y)
        {
            if (x.EndTime > y.EndTime)
                return 1;
            if (x.EndTime < y.EndTime)
                return -1;
            else
                return 0;
        }
    }
    public sealed class NetworkSimulator 
    {
        private static NetworkSimulator instance = null;
        private static readonly object padlock = new object();
        public PriorityQueue<CallStartedEvent> EventQueue { get; set; }
        private NetworkSimulator()
        {
            EventQueue = new PriorityQueue<CallStartedEvent>(new EventQueueComparer());
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