using System.Linq;

namespace VerticalHandoverPrediction
{

    //Refactor and make this a Singleton
    public class NonPredictiveJCAC : IJCAC
    {
        private readonly IHetNet _hetNet;

        private NonPredictiveJCAC(IHetNet hetNet) => _hetNet = hetNet;

        public static IJCAC Initialize(IHetNet hetNet) => new NonPredictiveJCAC(hetNet);

        //return JCAC object
        public bool AdmitCall(ICall call)
        {
            //If incoming call is on a mobile terminal already on another session
            if (call.MobileTerminal.State != MobileTerminalState.Idle)
            {
                //Find RAT accommodating current call
                var currentRAT = _hetNet.RATs
                    .FirstOrDefault(x => x.RATId == call.MobileTerminal.RATId);
                
                //Check if current RAT can admit incoming call in terms of the service and available bandwidth
                if(currentRAT.CanAccommodateCall(call))
                {
                    //Call RAT method to accommodate incoming call to an existing session
                    currentRAT.AdmitIncomingCallToOngoingSession(call);
                    //successfully admitted call
                    return true;
                }
                //If current RAT can't accommodate new call perform a vertical handover
                else 
                {
                    _hetNet.InitiateHandoverProcess(call);
                }
            }
            //New incoming call, no ongoing session
            else
            {
                //Find the candidate RATs from hetnet that can admit incoming call [support and available bbu]
                var candidateRATs = _hetNet.RATs
                    .Where(x => x.Services.Contains(call.Service))
                    .OrderBy(x => x.Services.Count)
                    .ToList();
                
                foreach (var rat in candidateRATs)
                {
                    if(rat.CanAccommodateCall(call))
                    {
                        //Start new session
                        rat.AdmitIncomingCallToNewSession(call);
                        break;
                    }
                    else 
                    {
                        //No RAT can accommodate the incoming call so drop call
                        //Keep Track of the number of calls dropped in the hetnet
                    }
                }

            }
            return true;
        }
    }
}