using System;
using System.Linq;

namespace VerticalHandoverPrediction
{
    public class Call : ICall
    {
        public Guid CallId { get; private set; }
        public IMobileTerminal MobileTerminal { get; private set; }
        public Service Service { get; private set; }

        //Inject the JCAC object into Call class to perform Algorithm
        //??? Use HetNet Object

        private Call(IMobileTerminal mobileTerminal, Service service)
        {
            CallId = Guid.NewGuid();
            MobileTerminal = mobileTerminal ?? throw new ArgumentNullException(nameof(mobileTerminal));
            Service = service;
        }

        public static Call InitiateCall(IMobileTerminal mobileTerminal, Service service, IJCAC jcac)
        {
            var call = new Call(mobileTerminal, service);
            
            //Perform Algorithm When A call is initiated, jcac object handles RAT selection algorithm
            jcac.AdmitCall(call, mobileTerminal);
            

            //****************************************** */
            //Perform the Algorithm RAT Selection Algorithm Here Before Recording The Call
            //****************************************** */

            //if (!mobileTerminal.IsOnActiveSession())
            //{

            //    mobileTerminal.SetMobileTerminalCurrentState(service);
            //    mobileTerminal.CurrentSession = CallSession.InitiateSession(mobileTerminal);
            //}
            //else
            //{
                //if on an active session
            //    mobileTerminal.SetMobileTerminalCurrentState(service);
            //    mobileTerminal.CurrentSession.UpdateCallSessionSequence(mobileTerminal.CurrentState);
            //}
            //mobileTerminal.CurrentSession.ActiveCalls.Add(call);
            return call;
        }

        public void TerminateCall(IMobileTerminal mobileTerminal, ICall call)
        {
            //Find Call From Session
            var callToTerminate = mobileTerminal.CurrentSession.ActiveCalls
                .FirstOrDefault(x => x.CallId == call.CallId);
            //Remove Call From Session
            mobileTerminal.CurrentSession.ActiveCalls.Remove(callToTerminate);
            //What happens to the call sequnce
            /*
                Only utilized bandwidth is affected, prediction and call sequence not affected
            */

            //Update utilized bandwidth pass the current session object
        }

    }
}