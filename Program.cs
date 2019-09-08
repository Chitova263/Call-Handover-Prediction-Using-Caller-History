using VerticalHandoverPrediction.Network;
using Serilog;
using VerticalHandoverPrediction.Simulator;
using VerticalHandoverPrediction.Utils;

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
            HetNet._HetNet.GenerateUsers(5);
            
            NetworkSimulator._NetworkSimulator.Run(100);
            NetworkSimulator._NetworkSimulator.Run(100);
            NetworkSimulator._NetworkSimulator.Run(100);
            NetworkSimulator._NetworkSimulator.Run(100);
           
            HetNet._HetNet.Dump();
        }
    }
}
