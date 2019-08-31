using System;
using VerticalHandoverPrediction.Utils;

namespace VerticalHandoverPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var mt = MobileTerminal.CreateMobileTerminal();
            Call.InitiateCall(mt, Service.Video);
            Call.InitiateCall(mt, Service.Voice);
            mt.Dump();
            mt.CurrentSession.TerminateSession(mt);
            System.Console.WriteLine("*****************************************************");
            mt.Dump();
            mt.CurrentSession.SessionDuration().Dump();
        }
    }
}
