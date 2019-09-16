using VerticalHandoverPrediction.Network;
using Serilog;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Utils;
using System;

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
  
            HetNet._HetNet.GenerateRats();

            HetNet._HetNet.GenerateUsers(20);
            
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            
            for (int i = 0; i < 10; i++)
            {
                NetworkSimulator._NetworkSimulator.Run(500, true, false);
            }

            NetworkSimulator._NetworkSimulator.UseCallLogs = false;
            
            NetworkSimulator._NetworkSimulator.Run(10, false, false);

            System.Console.WriteLine(HetNet._HetNet.CallsGenerated);
            System.Console.WriteLine("NonPredictive      Predictive");
            System.Console.Write(HetNet._HetNet.VerticalHandovers + "         ");
            
            NetworkSimulator._NetworkSimulator.Run(10, false, true);
            System.Console.Write(HetNet._HetNet.VerticalHandovers);

            System.Console.WriteLine();
        }
    }
}