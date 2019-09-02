using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalHandoverPrediction
{
    //HetNet contains a pool of RATs
    public class HetNet : IHetNet
    {
        public Guid HetNetId { get; private set; }
        public IList<RAT> RATs { get; private set; } = new List<RAT>();
        public static int NumberOfHandovers { get; private set; }
        public static int NumberOfCallsBlocked { get; private set; }
        public static int NumberOfGeneratedCalls { get; private set; }

        private HetNet(IList<RAT> rats)
        {
            HetNetId = Guid.NewGuid();
            RATs = rats;
        }

        public static HetNet InitializeHetNet(IList<RAT> rats)
        {
            var hetnet = new HetNet(rats);
            return hetnet;
        }

        public MobileTerminalState PredictNextState(ICall call)
        {
            throw new NotImplementedException();
        }

        public bool InitiateHandoverProcess(ICall call)
        {
            var previousRAT = RATs
                .FirstOrDefault(x => x.RATId == call.MobileTerminal.RATId);

            var previousSession = call.MobileTerminal.SessionId;

            //Find the current RAT
            //Find the services in the ongoing session
            var services = RATs
                .FirstOrDefault(x => x.RATId == call.MobileTerminal.RATId)
                .OngoingSessions
                .FirstOrDefault(x => x.SessionId == call.MobileTerminal.SessionId)
                .ActiveCalls
                .Select(x => x.Service)
                .ToList();

            //Add the service of new call
            services.Add(call.Service);

            //Find Candidate RATs that support the [handover session] For Handover
            var candidateRATs = RATs
                .Where(x => x.RATId != call.MobileTerminal.RATId
                    && x.Services.ToHashSet().IsSupersetOf(services))
                .OrderBy(x => x.Services.Count)
                .ToList();

            //Find RAT that has capacity to admit handover call
            var requiredBbu = 0;
            foreach (var service in services)
            {
                requiredBbu += service.ComputeRequiredCapacity();
            }

            //NB: session keeps the sameId for tracking through RATs
            //Perform handover proceedure
            foreach (var rat in candidateRATs)
            {
                //If there is enough network resources
                if (rat.AvailableCapacity() >= requiredBbu)
                {
                    //SuccessFull Handover
                    return Handover(call, previousRAT, rat);
                }
            }
            return false; //What does this mean //Drop The Call
        }

        private bool Handover(ICall call, IRAT sourceRAT, IRAT targetRAT)
        {
            //Find the current RAT
            var currentRAT = RATs.FirstOrDefault(x => x.RATId == call.MobileTerminal.RATId);
            //Find the current session
            var currentSession = currentRAT.OngoingSessions
                .FirstOrDefault(x => x.SessionId == call.MobileTerminal.SessionId);
            //Add method to free resources in source RAT
            sourceRAT.RemoveSessionFromSourceRAT(currentSession);
            //Transfer session 
            targetRAT.TransferSessionTo(currentSession, call); //Clean This

            //Admit the incoming call at the target RAT
            targetRAT.AdmitIncomingCallToOngoingSession(call);

            return true;
        }

        public bool InitiateHandoverPrediction(ICall call)
        {
            var previousRAT = RATs
                .FirstOrDefault(x => x.RATId == call.MobileTerminal.RATId);

            var previousSession = call.MobileTerminal.SessionId;

            var history =  call.MobileTerminal.CallHistoryLog
                .Select(x => x.SessionSequence)
                .T
        }
    }
}
