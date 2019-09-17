using VerticalHandoverPrediction.Network;
using Serilog;
using VerticalHandoverPrediction.Simulator;
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
  
            HetNet.Instance.GenerateRats();

            HetNet.Instance.GenerateUsers(20);
            
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
            Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            
            for (int i = 0; i < 10; i++)
            {
                NetworkSimulator._NetworkSimulator.Run(500, true, false);
            }

           
            
            while(true)
            {
                var input = int.Parse(Console.ReadLine());
                if(input == -1) break;

                for (int i = 0; i < 10; i++)
                {
                    NetworkSimulator._NetworkSimulator.Run(input, true, false);
                }

                NetworkSimulator._NetworkSimulator.UseCallLogs = false;
              
                //None Predictive Scheme
                NetworkSimulator._NetworkSimulator.Run(10, false, false);
                var calls = HetNet.Instance.CallsGenerated;
                var nonPredictive = HetNet.Instance.VerticalHandovers;
                //PredictiveScheme
                NetworkSimulator._NetworkSimulator.Run(10, false, true);
                var predictive = HetNet.Instance.VerticalHandovers;
                System.Console.WriteLine(calls + "   " + nonPredictive + "   "+ predictive);

                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/start.csv");
                Utils.CsvUtils._Instance.Clear($"{Environment.CurrentDirectory}/end.csv");
            }
        }
    }
}