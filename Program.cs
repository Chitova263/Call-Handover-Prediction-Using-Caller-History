using System.Collections.Generic;
using VerticalHandoverPrediction.Utils;

namespace VerticalHandoverPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var rats = new List<RAT>
            {
                RAT.CreateRAT(new List<Service>
                {
                    Service.Voice, Service.Data, Service.Video
                }, 3),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Voice
                }, 3),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 3),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Video, Service.Voice,
                }, 3),
            };

            //Initialize Network
            IHetNet network = HetNet.InitializeHetNet(rats);
            
            //network.Dump();
           
            var jcac = NonPredictiveJCAC.Initialize(network);

            var mt1 = MobileTerminal.CreateMobileTerminal(MobileTerminalModality.TrippleMode);
            var mt2 = MobileTerminal.CreateMobileTerminal(MobileTerminalModality.TrippleMode);

            var call1 = Call.InitiateCall(mt1, Service.Voice, jcac);
            var call2 = Call.InitiateCall(mt1, Service.Data, jcac);
           
            network.RATs.Dump();

            mt1.TerminateSession(network);
            
            System.Console.WriteLine("##########################################");

            network.RATs.Dump();

            System.Console.WriteLine("##########################################");
            mt1.Dump();
        }
    }
}
