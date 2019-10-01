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

            connection.On<dynamic, dynamic>("greeting", message => {
                return "Home";
            });
            connection.Listen();

                // HetNet.Instance.GenerateRats();
                // HetNet.Instance.GenerateMobileTerminals(50);
                
                // NetworkSimulator.Instance.GenerateCalls(500);
                
                // foreach (var log in HetNet.Instance.MobileTerminals.SelectMany(x => x.CallLogs))
                // {
                //     Utils.CsvUtils._Instance.Write<CallLogMap, CallLog>(
                //         log, 
                //         $"{Environment.CurrentDirectory}/calllogs.csv");
                // }

                // NetworkSimulator.Instance.Events.Clear();

                // var list = new int[]{100, 200, 300};
                // var results = new List<SimulationResults>();
                
                // foreach (var item in list)
                // {
                //     NetworkSimulator.Instance.GenerateCalls(item);
                    
                //     NetworkSimulator.Instance.SaveCallLogs = false;
                    
                //     //Non Predictive Scheme
                //     NetworkSimulator.Instance.Run(false);
                //     var nonPredictiveSchemeResults = new 
                //     {
                //         Calls = HetNet.Instance.CallsGenerated,
                //         NonPredictiveHandovers = HetNet.Instance.VerticalHandovers,
                //         NonPredictiveBlockedCalls = HetNet.Instance.BlockedCalls,
                //         TotalSessions = HetNet.Instance.TotalSessions,
                //     };

                //     //Predictive Scheme
                //     NetworkSimulator.Instance.Run(true);
                //     var predictiveSchemeResults = new 
                //     {
                //         PredictiveHandovers = HetNet.Instance.VerticalHandovers,
                //         PredictiveBlockedCalls = HetNet.Instance.BlockedCalls,
                //         FailedPredictions  = HetNet.Instance.FailedPredictions,
                //         SuccessfulPredictions = HetNet.Instance.SuccessfulPredictions,
                //     };

                //     var simulationResults = new SimulationResults
                //     {
                //         Calls = nonPredictiveSchemeResults.Calls,
                //         TotalSessions = nonPredictiveSchemeResults.TotalSessions,
                //         NonPredictiveHandovers = nonPredictiveSchemeResults.NonPredictiveHandovers,
                //         NonPredictiveBlockedCalls = nonPredictiveSchemeResults.NonPredictiveBlockedCalls,
                //         PredictiveHandovers = predictiveSchemeResults.PredictiveHandovers,
                //         PredictiveBlockedCalls = predictiveSchemeResults.PredictiveBlockedCalls,
                //         FailedPredictions = predictiveSchemeResults.FailedPredictions,
                //         SuccessfulPredictions = predictiveSchemeResults.SuccessfulPredictions
                //     };

                //     Utils.CsvUtils._Instance.Write<SimulationResultsMap, SimulationResults>(
                //         simulationResults, $"{Environment.CurrentDirectory}/SimResults.csv"
                //     );

                //     results.Add(simulationResults);

                //     HetNet.Instance.Reset(); 
                //     NetworkSimulator.Instance.Events.Clear();            
                // }    
        }
       
    }
}
