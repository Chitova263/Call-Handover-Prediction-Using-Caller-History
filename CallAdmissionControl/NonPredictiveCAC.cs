using VerticalHandoverPrediction.CallSession;
using VerticalHandoverPrediction.Network;
using System.Linq;
using VerticalHandoverPrediction.Mobile;

//Converted to a singleton
namespace VerticalHandoverPrediction.CallAdmissionControl
{

    public class NonPredictiveCAC : ICAC
    {
        private NonPredictiveCAC()
        {
            
        }

        public static ICAC StartCACAlgorithm()
        {
            return new NonPredictiveCAC();
        }
        
        public void AdmitCall(ICall call)
        {
            //Find the mobile terminal involved in call [From The HetNet]
            var mobileTerminal = HetNet._HetNet.MobileTerminals
                .FirstOrDefault(m => m.MobileTerminalId == call.MobileTerminalId);

            IRat currentRat = default(IRat);

            //If incoming call is on a mobile terminal on an active session
            if(mobileTerminal.State != MobileTerminalState.Idle)
            {
                //Find the Rat accommodating the current session
                var currentSession = HetNet._HetNet.Rats
                    .SelectMany(x => x.OngoingSessions)
                    .FindSessionWithId(mobileTerminal.SessionId);



                    //.SelectMany(x => x.OngoingSessions
                        //how does the call have a sessionId here/Call is not yet admmited to a session
                    //    .FindSessionWithId(mobileTerminal.SessionId)
                    //)
                    //.FirstOrDefault();
                
                currentRat = HetNet._HetNet.Rats
                    .FirstOrDefault(x => x.RatId == currentSession.RatId);
                
                //Check if the current Rat can admit the incoming call
                if(currentRat.CanAccommodateCall(call))
                {
                    //Accommodate incoming call to the currentSession
                    currentRat.AdmitIncomingCallToOngoingSession(call,currentSession,mobileTerminal);
                    //Call Admission Successful End CAC
                    return;
                }
                //If current Rat cannot accomodate the call we need to handover the session to another RAT
                else
                {
                    StartHandoverProcess(call, currentSession, currentRat, mobileTerminal);
                }
            } 
            //new call, no ongoing session
            else 
            {
                //Non Predictive algorithm : => Swap this with predictive code
                var candidateRats = HetNet._HetNet.Rats
                    .Where(x => x.Services.Contains(call.Service))
                    .OrderBy(x => x.Services.Count)
                    .ToList();
                
                foreach (var rat in candidateRats)
                {
                    if(rat.CanAccommodateCall(call))
                    {
                        rat.AdmitIncomingCallToNewSession(call, mobileTerminal);
                        break;
                    } else 
                    {
                        //All the RATs cant accommodate new call
                        HetNet._HetNet.BlockedCalls++;
                    }
                }
            }
        }

        public void StartHandoverProcess(ICall call, ISession currentSession, IRat sourceRat, IMobileTerminal mobileTerminal)
        {
            //Find services in ongoing session
            var services = currentSession.ActiveCalls
                .Select(x => x.Service)
                .ToList();
            //Add the service of new call
            services.Add(call.Service);

            //Find Candidate Rats that support required services
            var candidateRats = HetNet._HetNet.Rats
                .Where(x => x.RatId != currentSession.RatId
                    && x.Services.ToHashSet().IsSupersetOf(services))
                .OrderBy(x => x.Services.Count())
                .ToList();
            
            //From the candidate Rats find rat that has enough capacity to admit new session
            var requiredBbu = 0;
            foreach (var service in services)
            {
                requiredBbu += service.ComputeRequiredCapacity();
            }

            //NB: session keeps the same SessionId for tracking through RATs
            //1. Transfer ongoing session to target RAT
            //2. Add new call to ongoing session at target rat
            foreach (var destinationRat in candidateRats)
            {
                //If there is Rat with enough network resources
                if (destinationRat.AvailableCapacity() >= requiredBbu)
                {
                    InitiateHandover(call, sourceRat, destinationRat, currentSession, mobileTerminal);
                    //Handover is successfull
                    HetNet._HetNet.VerticalHandovers++;
                    return;
                }
            }
            //If you reach here block the incoming call
            HetNet._HetNet.BlockedCalls++;
            return;
        }

        private void InitiateHandover(ICall call, IRat sourceRat, IRat destinationRat, ISession currentSession, IMobileTerminal mobileTerminal)
        {
            //Remove session from source Rat
            sourceRat.RemoveSessionFromRat(currentSession);
            //Transfer session to target Rat
            destinationRat.TransferSessionToRat(currentSession);
            //Admit the call new call to 
            destinationRat.AdmitIncomingCallToOngoingSession(call, currentSession, mobileTerminal);
        }
    }
}