using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;
using VerticalHandoverPrediction.Utils;
using VerticalHandoverPrediction.Mobile;
using Serilog;

namespace VerticalHandoverPrediction
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
                
            var rats = new List<Rat>
            {
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Data, Service.Video
                }, 10),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice
                }, 10),
                Rat.CreateRat(new List<Service>
                {
                    Service.Voice, Service.Data
                }, 10),
                Rat.CreateRat(new List<Service>
                {
                    Service.Video, Service.Voice,
                }, 10),
            };

            foreach (var rat in rats)
            {
                HetNet._HetNet.Rats.Add(rat);
            }

            //HetNet._HetNet.Rats.Dump();

            var mt = MobileTerminal.CreateMobileTerminal(Modality.TrippleMode);
            var mt3 = MobileTerminal.CreateMobileTerminal(Modality.TrippleMode);
            var mt2 = MobileTerminal.CreateMobileTerminal(Modality.TrippleMode);
            var mt1 = MobileTerminal.CreateMobileTerminal(Modality.TrippleMode);
            HetNet._HetNet.MobileTerminals.Add(mt);
            HetNet._HetNet.MobileTerminals.Add(mt3);
            HetNet._HetNet.MobileTerminals.Add(mt2);
            HetNet._HetNet.MobileTerminals.Add(mt1);

            //HetNet._HetNet.MobileTerminals.Dump();

            mt.Dump();

            var call1 = Call.StartCall(mt.MobileTerminalId, Service.Voice);
            var call2 = Call.StartCall(mt.MobileTerminalId, Service.Video);
            var call3 = Call.StartCall(mt.MobileTerminalId, Service.Data);
            
            
            mt.TerminateCall(call1.CallId);
            mt.TerminateCall(call3.CallId);
            mt.TerminateCall(call2.CallId);

            //Consider keeping a list of calls made by the mobile terminal
            
            mt.Dump();

            //Prediction Happens Now
            Call.StartCall(mt.MobileTerminalId, Service.Voice); 

            HetNet._HetNet.Rats.Dump();
        }
    }
}
