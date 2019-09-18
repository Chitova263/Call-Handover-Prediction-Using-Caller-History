using VerticalHandoverPrediction.Network;
using Serilog;
using VerticalHandoverPrediction.Simulator;
using System;
using System.Collections.Generic;

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
  
            HetNet.Instance.GenerateRats();

            Console.WriteLine(">>>> Enter the number of users: ");
            var numberOfUsers = int.Parse(Console.ReadLine().Trim());
            HetNet.Instance.GenerateUsers(numberOfUsers);
            
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            
            Console.WriteLine(">>>>> Enter the number of call history records to generate: ");
            var numberOfRecords = int.Parse(Console.ReadLine().Trim());
            NetworkSimulator._NetworkSimulator.Run(numberOfRecords, true, false);

            Console.WriteLine(">>>>>> Enter the list of number of calls to generate for simulation e.g 10 20 30");
            var input = Console.ReadLine().Split(" ");
            var calls = new List<int>();
            foreach (var item in input)
            {
                calls.Add(int.Parse(item));
            }

            foreach (var numberOfCalls in calls)
            {
                NetworkSimulator._NetworkSimulator.Run(numberOfCalls, true, false);
                
                NetworkSimulator._NetworkSimulator.UseCallLogs = false;
              
                Log.Information("Running Non Predictive Scheme");
                NetworkSimulator._NetworkSimulator.Run(default(int), false, false);
                var results = new SimulationResults
                {
                    Calls = HetNet.Instance.CallsGenerated,
                    Handovers = HetNet.Instance.VerticalHandovers,
                    BlockedCalls = HetNet.Instance.BlockedCalls,
                    FailedPredictions  = HetNet.Instance.FailedPredictions,
                    SuccessfulPredictions = HetNet.Instance.SuccessfulPredictions,
                    TotalSessions = HetNet.Instance.TotalSessions,
                };

                Utils.CsvUtils._Instance.Write<SimulationResultsMap, SimulationResults>(results, $"{Environment.CurrentDirectory}/nonpredictiveresults.csv");
                
                Log.Information("Running Predictive Scheme");
                NetworkSimulator._NetworkSimulator.Run(default(int), false, true);
                results = new SimulationResults
                {
                    Handovers = HetNet.Instance.VerticalHandovers,
                    BlockedCalls = HetNet.Instance.BlockedCalls,
                    FailedPredictions  = HetNet.Instance.FailedPredictions,
                    SuccessfulPredictions = HetNet.Instance.SuccessfulPredictions,
                    Calls = HetNet.Instance.CallsGenerated,
                    TotalSessions = HetNet.Instance.TotalSessions,
                };
                
                Utils.CsvUtils._Instance.Write<SimulationResultsMap, SimulationResults>(results, $"{Environment.CurrentDirectory}/predictiveresults.csv");
                
                //Clear Events -- consider storing these events in memory as an optimization
                //No need to store them on csv file
                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            }
            Log.Information("Simulation Ended");
        }
    }
}