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

            HetNet._HetNet.GenerateUsers(10);
            
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            
            for (int i = 0; i < 10; i++)
            {
                NetworkSimulator._NetworkSimulator.Run(200, true, false);
            }
            NetworkSimulator._NetworkSimulator.UseCallLogs = false;
            
            NetworkSimulator._NetworkSimulator.Run(10, false, false);
            HetNet._HetNet.Dump();
            NetworkSimulator._NetworkSimulator.UseCallLogs = false;

            NetworkSimulator._NetworkSimulator.Run(10, false, true);
            HetNet._HetNet.Dump();
        }
    }
}