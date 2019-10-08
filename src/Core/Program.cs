namespace VerticalHandoverPrediction
{
    using VerticalHandoverPrediction.Network;
    using Serilog;
    using VerticalHandoverPrediction.Simulator;
    using System;
    using VerticalHandoverPrediction.Mobile;
    using System.Linq;
    using ElectronCgi.DotNet;
    using System.Collections.Generic;
    using Electron;
    using VerticalHandoverPrediction.CallSession;

    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
                
            var connection = new ConnectionBuilder()
                .WithLogging()
                .Build();

            // var result = NetworkSimulator.Instance.Predict(new PredictionParameters{
            //     Service = CallSession.Service.Data,
            //     MobileTerminalId = Guid.Parse("9e89f07c-ac04-4e0e-b113-fe418996b5cc")
            // });

            // result.Dump();

            connection.On<dynamic, List<Guid>>("getusers", request => {
                return NetworkSimulator.Instance.LoadUsers();
            });

            connection.On<dynamic,dynamic>("predict", request => {
                Service service = default(Service);
                switch ((string)request.service)
                {
                    case "Voice":
                        service = Service.Voice;
                        break;
                    case "Data":
                        service = Service.Data;
                        break;
                    case "Video":
                        service = Service.Video;
                        break;
                };

                return NetworkSimulator.Instance.Predict(new PredictionParameters{ Service = service, Id = (string)request.mobileTerminalId});
            });

            connection.On<SimulationParameters,dynamic>("results", request => {
                
                HetNet.Instance.CallerHistory.Clear();
                HetNet.Instance.GenerateRats((int)request.Capacity.c1, (int)request.Capacity.c2, (int)request.Capacity.c3, (int)request.Capacity.c4);
                HetNet.Instance.GenerateMobileTerminals(20);
                
                NetworkSimulator.Instance.GenerateCalls(500);
                
                foreach (var log in HetNet.Instance.MobileTerminals.SelectMany(x => x.CallLogs))
                {
                    Utils.CsvUtils._Instance.Write<CallLogMap, CallLog>(
                         log, 
                         $"{Environment.CurrentDirectory}/calllogs.csv");
                    //load history in memory    
                    HetNet.Instance.CallerHistory.Add(log);
                }

                NetworkSimulator.Instance.Events.Clear();

                var list = request.Calls.Split(",").Select(x => int.Parse(x));
                var results = new List<SimulationResults>();
                
                foreach (var item in list)
                {
                    NetworkSimulator.Instance.GenerateCalls(item);
                    
                    NetworkSimulator.Instance.SaveCallLogs = false;
                    
                    //Non Predictive Scheme
                    NetworkSimulator.Instance.Run(false);
                    var nonPredictiveSchemeResults = new 
                    {
                        Calls = HetNet.Instance.CallsGenerated,
                        NonPredictiveHandovers = HetNet.Instance.VerticalHandovers,
                        NonPredictiveBlockedCalls = HetNet.Instance.BlockedCalls,
                        TotalSessions = HetNet.Instance.TotalSessions,
                        HetNet.Instance.DataCallsGenerated,
                        HetNet.Instance.DataHandovers,
                        HetNet.Instance.VideoCallsGenerated,
                        HetNet.Instance.VideoHandovers,
                        HetNet.Instance.VoiceCallsGenerated,
                        HetNet.Instance.VoiceHandovers,
                    };

                    //Predictive Scheme
                    NetworkSimulator.Instance.Run(true);
                    var predictiveSchemeResults = new 
                    {
                        PredictiveHandovers = HetNet.Instance.VerticalHandovers,
                        PredictiveBlockedCalls = HetNet.Instance.BlockedCalls,
                        FailedPredictions  = HetNet.Instance.FailedPredictions,
                        SuccessfulPredictions = HetNet.Instance.SuccessfulPredictions,
                        PercentageVoiceAvoided =(double)(nonPredictiveSchemeResults.VoiceHandovers - HetNet.Instance.VoiceHandovers)/nonPredictiveSchemeResults.VoiceHandovers,
                        PercentageDataAvoided = (double)(nonPredictiveSchemeResults.DataHandovers - HetNet.Instance.DataHandovers )/nonPredictiveSchemeResults.DataHandovers,
                        PercentageVideoAvoided = (double)(nonPredictiveSchemeResults.VideoHandovers - HetNet.Instance.VideoHandovers )/nonPredictiveSchemeResults.VideoHandovers,
                        PercentageTotalAvoided = (double)(nonPredictiveSchemeResults.NonPredictiveHandovers - HetNet.Instance.VerticalHandovers)/nonPredictiveSchemeResults.NonPredictiveHandovers,
                        
                    };

                    var simulationResults = new SimulationResults
                    {
                        Calls = nonPredictiveSchemeResults.Calls,
                        TotalSessions = nonPredictiveSchemeResults.TotalSessions,
                        NonPredictiveHandovers = nonPredictiveSchemeResults.NonPredictiveHandovers,
                        NonPredictiveBlockedCalls = nonPredictiveSchemeResults.NonPredictiveBlockedCalls,
                        PredictiveHandovers = predictiveSchemeResults.PredictiveHandovers,
                        PredictiveBlockedCalls = predictiveSchemeResults.PredictiveBlockedCalls,
                        FailedPredictions = predictiveSchemeResults.FailedPredictions,
                        SuccessfulPredictions = predictiveSchemeResults.SuccessfulPredictions,
                        DataAvoided = Math.Round(predictiveSchemeResults.PercentageDataAvoided*100,2),
                        VideoAvoided = Math.Round(predictiveSchemeResults.PercentageVideoAvoided*100,2),
                        VoiceAvoided = Math.Round(predictiveSchemeResults.PercentageVoiceAvoided*100,2),
                        TotalAvoided = Math.Round(predictiveSchemeResults.PercentageTotalAvoided*100,2),
                        DataCalls = nonPredictiveSchemeResults.DataCallsGenerated,
                        VoiceCalls = nonPredictiveSchemeResults.VoiceCallsGenerated,
                        VideoCalls = nonPredictiveSchemeResults.VideoCallsGenerated,
                        PredictiveVoiceHandovers = HetNet.Instance.VoiceHandovers,
                        PredictiveVideoHandovers = HetNet.Instance.VideoHandovers,
                        PredictiveDataHandovers =  HetNet.Instance.DataHandovers,
                        NonPredictiveVoiceHandovers = nonPredictiveSchemeResults.VoiceHandovers,
                        NonPredictiveVideoHandovers = nonPredictiveSchemeResults.VideoHandovers,
                        NonPredictiveDataHandovers =  nonPredictiveSchemeResults.DataHandovers,
                    };

                    // Utils.CsvUtils._Instance.Write<SimulationResultsMap, SimulationResults>(
                    //     simulationResults, $"{Environment.CurrentDirectory}/SimResults.csv"
                    // );

                    results.Add(simulationResults);

                    HetNet.Instance.Reset(); 
                    NetworkSimulator.Instance.Events.Clear();            
                } 

                //Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/SimResults.csv"); 
                //Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/calllogs.csv"); 
                
                return results;
            });
            connection.Listen();  
        }
       
    }
}
