using VerticalHandoverPrediction.Network;
using Serilog;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Utils;
using ClosedXML;
using ClosedXML.Excel;
using System.Linq;
using System.Data;
using System.IO;
using CsvHelper;
using VerticalHandoverPrediction.Mobile;
using System;
using System.Collections.Generic;
using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            //Generate Rats    
            HetNet._HetNet.GenerateRats();

            //Generate users
            //HetNet._HetNet.GenerateUsers(10);
            
            //NetworkSimulator._NetworkSimulator.Run(100);
            //NetworkSimulator._NetworkSimulator.Run(100);
            //NetworkSimulator._NetworkSimulator.Run(100);
            //NetworkSimulator._NetworkSimulator.Run(100);
            //NetworkSimulator._NetworkSimulator.Run(100);
            //NetworkSimulator._NetworkSimulator.Run(100);
            
            var user = MobileTerminal.CreateMobileTerminal(Modality.TrippleMode);
            user.MobileTerminalId = Guid.Parse("956083b9-97a3-4d73-acc4-fd77b4fd4661");
            HetNet._HetNet.AddMobileTerminals(new List<IMobileTerminal>{user});
            var call = Call.StartCall(user.MobileTerminalId, Service.Voice);
            var evt = new CallStartedEvent(DateTime.Now, call);
            new Cac.Cac().AdmitCall(evt);
            
            var history = HetNet._HetNet.MobileTerminals
                .Select(x => x.CallHistoryLogs)
                //.OrderBy(x => x.Select(x => x.UserId))
                .ToList();
    
            var str = @"/Users/DjMadd/Documents/Thesis/VerticalHandoverPrediction";        
            //var writer = new StreamWriter($"{str}/jkby.csv");
           // var CsvWriter = new CsvWriter(writer);
           // CsvWriter.Configuration.Delimiter = ",";
            //CsvWriter.Configuration.HasHeaderRecord = true;
           // CsvWriter.Configuration.AutoMap<CallLog>();

           // foreach (var item in history)
           // {
          //      CsvWriter.WriteRecords(item);
          //  }

            
            using (var reader = new StreamReader($"{str}/jkby.csv"))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<CallLog>();
                var x = records.ToList();
                int c = 0;
            }

            
           
           

           HetNet._HetNet.Dump();
        }
    }
}
