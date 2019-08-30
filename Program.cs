using System;
using HandoverPrediction.Utils;

namespace HandoverPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var mt = new MobileTerminal(Guid.NewGuid());
            //mt.Dump();
            var call = Call.InitiateCall(mt, Service.Voice);
            
        }
    }
}
