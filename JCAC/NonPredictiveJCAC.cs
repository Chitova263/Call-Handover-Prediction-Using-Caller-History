using System;
using System.Linq;

namespace VerticalHandoverPrediction
{

    //Refactor and make this a Singleton
    public class NonPredictiveJCAC : IJCAC
    {
        public IHetNet HetNet { get; private set; }

        private NonPredictiveJCAC(IHetNet hetNet)
        {
            this.HetNet = hetNet ?? throw new System.ArgumentNullException(nameof(HetNet));
        }

        public static IJCAC Initialize(IHetNet hetNet)
        {
            var network = new NonPredictiveJCAC(hetNet);
            return network;
        }

        //return JCAC object
        public bool AdmitCall(ICall call, IMobileTerminal mobileTerminal)
        {
            if(mobileTerminal.IsOnActiveSession())
            {
                //Find RAT accommodating current session
                var rat =  HetNet.RATs
                    .FirstOrDefault(x => x.RATId == mobileTerminal.CurrentSession.RATId);
                if(rat.CanAccommodateCall(call))
                {
                    //Admit call to RAT
                    mobileTerminal.SetMobileTerminalCurrentState(call.Service);
                    mobileTerminal.CurrentSession.ActiveCalls.Add(call);
                    rat.AdmitCall(mobileTerminal.CurrentSession);
                } else {
                    //Handover call if RAT cannot accommodate
                }
            } else {
                //Not on an active session
                //History can be empty?? History can Exist??

                /*       
                voice
                data
                voice data
                voice video data
                 */
                var rats = HetNet.RATs
                    .Where(x => x.Services.Contains(call.Service))
                    .OrderBy(x => x.Services.Count)
                    .ToList();
                
                foreach (var rat in rats)
                {
                    if(rat.CanAccommodateCall(call))
                    {
                        //Admit Call
                        mobileTerminal.SetMobileTerminalCurrentState(call.Service);
                        mobileTerminal.CurrentSession = CallSession.InitiateSession(mobileTerminal, rat.RATId);
                        mobileTerminal.CurrentSession.ActiveCalls.Add(call);
                        rat.AdmitCallSession(mobileTerminal.CurrentSession);
                        break;
                    } else {
                        //Drop the call 
                    }
                }


            }

            //if history is empty and its a new call just find suitable RAT to admit call
            //if (!mobileTerminal.CallHistoryLog.Any() && !mobileTerminal.IsOnActiveSession())
            //{
                //Find RAT in hetnet to admit service type of call not predicting yet
                //var rat = this.HetNet.RATs
                //    .FirstOrDefault(x => x.Services.Contains(call.Service));

                //mobileTerminal.SetMobileTerminalCurrentState(call.Service);
                //mobileTerminal.CurrentSession = CallSession.InitiateSession(mobileTerminal);
                //rat.AdmitCallSession(mobileTerminal.CurrentSession);

                //mobileTerminal.CurrentSession.ActiveCalls.Add(call);
            //}
            return true;
        }

        //Implement JCAC algorithm
        /*
            takes in a call => if call is on 
         */


    }
}