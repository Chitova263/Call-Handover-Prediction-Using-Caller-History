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
            HetNet._HetNet.GenerateUsers(12);

            //Generate Calls
            //Use Simulator Instance to generate calls
            /*
                check list of active calls => if call service already exists just reject or discard the call object
            */
            
            NetworkSimulator._NetworkSimulator.Run(100);

            HetNet._HetNet.Dump();
        }
    }
}
