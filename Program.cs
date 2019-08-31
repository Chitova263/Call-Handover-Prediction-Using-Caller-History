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
            
            
            network.Dump();
           

            //var mt = MobileTerminal.CreateMobileTerminal();
            //Call.InitiateCall(mt, Service.Video);
            //Call.InitiateCall(mt, Service.Voice);
            //mt.Dump();
            //mt.CurrentSession.TerminateSession(mt);
            //System.Console.WriteLine("*****************************************************");
            //mt.Dump();
            //mt.CurrentSession.SessionDuration().Dump();
        }
    }
}
