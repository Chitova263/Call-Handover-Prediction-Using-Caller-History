using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.Utils;
using System.Linq;

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
                }, 100),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Voice
                }, 100),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 100),
                RAT.CreateRAT(new List<Service>
                {
                    Service.Video, Service.Voice,
                }, 100),
            };

            //Initialize Network
            var network = HetNet.InitializeHetNet(rats);
            
            //network.Dump();
           
            var jcac = PredictiveJCAC.Initialize(network);

            
            var mt1 = MobileTerminal.CreateMobileTerminal();
            var mt2 = MobileTerminal.CreateMobileTerminal();

            Call.InitiateCall(mt1, Service.Voice, jcac);
            Call.InitiateCall(mt2, Service.Voice, jcac);
           
            network.Dump();

            //Call.InitiateCall(mt, Service.Voice);
            //mt.Dump();
            //mt.CurrentSession.TerminateSession(mt);
            //System.Console.WriteLine("*****************************************************");
            //mt.Dump();
            //mt.CurrentSession.SessionDuration().Dump();
        }
    }
}
